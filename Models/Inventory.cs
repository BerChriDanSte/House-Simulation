namespace HouseSimulation.Models;

public class Inventory
{
    private readonly List<Item> _items = [];

    public IReadOnlyList<Item> Items => _items.AsReadOnly();

    public void AddItem(Item item)
    {
        _items.Add(item);
        Console.WriteLine($"Picked up: {item.Name}");
    }

    public bool HasItem(string name)
    {
        return _items.Any(i =>
            i.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public Item? GetItem(string name)
    {
        return _items.FirstOrDefault(i =>
            i.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public int CountTruthFragments()
    {
        return _items.Count(i => i is TruthFragment);
    }

    public void ListItems()
    {
        if (!_items.Any())
        {
            Console.WriteLine("Inventory is empty.");
            return;
        }

        Console.WriteLine("\nInventory:");
        foreach (var item in _items)
        {
            Console.WriteLine($" - {item.Name}: {item.Description}");
        }
    }
}