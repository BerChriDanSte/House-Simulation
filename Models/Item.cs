namespace HouseSimulation.Models;

public abstract class Item(string name, string description)
{
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;

    public abstract void Use(Character character);
}