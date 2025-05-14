namespace HouseSimulation.Models;

public class Room
{
    public string Name { get; }
    private string Description { get; }
    public bool IsDark { get; set; } = false;

    private readonly List<Item> _items = [];
    private readonly Dictionary<string, Connection> _connections = new();

    public Room(string name, string description, bool isDark = false)
    {
        Name = name;
        Description = description;
        IsDark = isDark;
    }

    public void AddItem(Item item) => _items.Add(item);

    public void AddConnection(string direction, Connection connection)
        => _connections[direction.ToLower()] = connection;

    public Connection? GetConnection(string direction)
    {
        _connections.TryGetValue(direction.ToLower(), out var conn);
        return conn;
    }
    
    public void Describe(Character character)
    {
        Console.WriteLine($"\n{Description}");

        if (IsDark && !character.HasItem("Flashlight"))
        {
            Console.WriteLine("It's too dark to see anything.");
            return;
        }

        if (_items.Count != 0)
        {
            Console.WriteLine("You see:");
            foreach (var item in _items)
            {
                Console.WriteLine($" - {item.Name}");
            }
        }

        var exits = _connections.Keys.ToList();
        if (exits.Count > 0)
        {
            Console.WriteLine($"Exits: {string.Join(", ", exits)}");
        }

        if (character.IntrusiveThoughtsManager?.CurrentEffect == IntrusiveThoughtEffect.FakeExit)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("...Or is there a glowing door to the west? No. Just your mind playing tricks.");
            Console.ResetColor();
        }
    }

    public bool IsVisible(Character character)
    {
        return !IsDark || character.HasItem("Flashlight");
    }

    public Item? TakeItem(string itemName)
    {
        var item = _items.FirstOrDefault(i =>
            i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));

        if (item != null)
        {
            _items.Remove(item);   
        }

        return item;
    }
}