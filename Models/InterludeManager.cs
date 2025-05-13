namespace HouseSimulation.Models;

public class InterludeManager
{
    private readonly Dictionary<int, string> _interludes = new()
    {
        [3] = "“Therapy taught me to listen to silence. I hated it.”",
        [7] = "“You mask pain with purpose. Is that noble or naive?”",
        [12] = "“I grieve different. But you already knew that.”",
        [16] = "“You promised you'd break the cycle. Why are your hands still shaking?”",
        [20] = "“Mother was sober. But I drank her shame anyway.”",
        [25] = "“You called it growth. But it looked like retreat.”",
        [30] = "“Even saviors need saving when the lights go out.”",
        [35] = "“What if healing means admitting you were wrong?”"
    };

    public void CheckAndPlay(int turnCount)
    {
        if (_interludes.TryGetValue(turnCount, out var message))
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"\n[INTERLUDE] {message}\n");
            Console.ResetColor();
        }
    }
}