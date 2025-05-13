namespace HouseSimulation.Models;

public class FlashbackToken(string name, string description, InterludeManager interludeManager)
    : Item(name, description)
{
    public override void Use(Character character)
    {
        Console.WriteLine("\nYou hold the FlashbackToken in your hand, and the world around you blurs.");

        interludeManager.CheckAndPlay(0);  // u can set a specific turn count or condition if needed
    }
}