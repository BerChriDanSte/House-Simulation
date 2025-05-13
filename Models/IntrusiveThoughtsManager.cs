namespace HouseSimulation.Models;

public class IntrusiveThoughtsManager
{
    private record Thought(string Text, IntrusiveThoughtEffect Effect);

    private static readonly List<Thought> Thoughts =
    [
        new("“They told you to man up — but what if you never learned how?”", IntrusiveThoughtEffect.FreezePlayer),
        new("“Your silence isn’t healing. It’s hiding.”", IntrusiveThoughtEffect.HideInventory),
        new("“That wasn’t love. That was survival pretending.”", IntrusiveThoughtEffect.ConfuseDirection),
        new("“You still hear his voice when you breathe slow, don’t you?”", IntrusiveThoughtEffect.FakeExit),
        new("“You said you were better. Why’d your hands shake then?”", IntrusiveThoughtEffect.None),
        new("“No one clapped for your progress. So you clapped alone.”", IntrusiveThoughtEffect.FreezePlayer)
    ];

    private readonly Random _random = new();
    public IntrusiveThoughtEffect CurrentEffect { get; private set; } = IntrusiveThoughtEffect.None;

    public void TriggerIntrusiveThoughts(int turnCount)
    {
        if (turnCount % 5 == 0)
        {
            var thought = Thoughts[_random.Next(Thoughts.Count)];
            CurrentEffect = thought.Effect;
            DisplayIntrusiveThought(thought.Text);
        }
        else
        {
            CurrentEffect = IntrusiveThoughtEffect.None;
        }
    }

    private void DisplayIntrusiveThought(string thought)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"\n[INTRUSIVE THOUGHT] {thought}\n");
        Console.ResetColor();
    }
}