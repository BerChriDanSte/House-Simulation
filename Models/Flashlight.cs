namespace HouseSimulation.Models;

public class Flashlight() 
    : Item("Flashlight", "Used to light up dark rooms.")
{
    public override void Use(Character character)
    {
        Console.WriteLine("You switch on the flashlight. Shadows retreat.");
    }
}