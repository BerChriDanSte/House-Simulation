namespace HouseSimulation.Models;

public class TruthFragment(string description) : Item("Truth Fragment", description)
{
    public override void Use(Character character)
    {
        if (character.CurrentRoom.Name == "The Conscience Room" &&
            character.Inventory.CountTruthFragments() >= 3)
        {
            Console.WriteLine("The Truth Fragments converge. You see clearly now...");
            character.WinGame();
        }
        else
        {
            Console.WriteLine("The fragment pulses. Memories flood in...");
        }
    }
}