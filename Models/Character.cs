namespace HouseSimulation.Models;

public class Character
{
    public Room CurrentRoom { get; set; }
    public Inventory Inventory { get; private set; } = new();
    public InterludeManager? InterludeManager { get; set; }
    public IntrusiveThoughtsManager? IntrusiveThoughtsManager { get; private set; }
    private readonly Action? _onWin;
    private List<Room> VisitedRooms { get; set; } = [];

    public Character(
        Room startRoom,
        IntrusiveThoughtsManager intrusiveThoughtsManager,
        InterludeManager? interludeManager = null,
        Action? onWinCallback = null)
    {
        CurrentRoom = startRoom;
        InterludeManager = interludeManager;
        IntrusiveThoughtsManager = intrusiveThoughtsManager;
        _onWin = onWinCallback;
        VisitedRooms.Add(startRoom);
    }

    public void WinGame()
    {
        _onWin?.Invoke();
    }

    public void Move(string direction)
    {
        var connection = CurrentRoom.GetConnection(direction);
        if (connection == null)
        {
            Console.WriteLine("You can't go that way.");
            return;
        }

        if (connection.IsLocked)
        {
            if (!string.IsNullOrEmpty(connection.RequiredItemName) &&
                Inventory.HasItem(connection.RequiredItemName))
            {
                Console.WriteLine($"You use the {connection.RequiredItemName} to unlock the path.");
                connection.Unlock();
            }
            else
            {
                Console.WriteLine("That way is locked. You may need a specific item.");
                return;
            }
        }

        CurrentRoom = connection.Destination;
        if (!VisitedRooms.Contains(CurrentRoom))
            VisitedRooms.Add(CurrentRoom);

        Console.WriteLine($"\n== {CurrentRoom.Name} ==");
        CurrentRoom.Describe(this);
    }

    public void TakeItem(string itemName)
    {
        if (!CurrentRoom.IsVisible(this))
        {
            Console.WriteLine("You can't see anything to pick up.");
            return;
        }

        var item = CurrentRoom.TakeItem(itemName);
        if (item != null)
        {
            Inventory.AddItem(item);
            Console.WriteLine($"You picked up: {item.Name}");
        }
        else
        {
            Console.WriteLine("That item isn't here.");
        }
    }

    public void UseItem(string itemName)
    {
        var item = Inventory.GetItem(itemName);
        if (item != null)
        {
            item.Use(this);
        }
        else
        {
            Console.WriteLine("You don't have that item.");
        }
    }

    public void ShowInventory()
    {
        Console.WriteLine("\nInventory:");
        foreach (var item in Inventory.Items)
        {
            Console.WriteLine($"- {item.Name}: {item.Description}");
        }
    }

    public bool HasItem(string itemName) =>
        Inventory.HasItem(itemName);

    public bool CanSee()
    {
        return !CurrentRoom.IsDark || Inventory.HasItem("Flashlight");
    }
}
