namespace HouseSimulation.Models;


public class Game
{
    private Character _player = null!;
    private GameState _state = GameState.Playing;
    private int _turnCount = 0;
    private const int MaxTurns = 40;
    private readonly IntrusiveThoughtsManager _intrusiveThoughtsManager = new();
    private readonly InterludeManager _interludes = new();


    public void Start()
    {
        Console.WriteLine("Welcome to the Memory Maze.");
        Setup();

        while (_state == GameState.Playing)
        {
            Console.Write("\n> ");
            var input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                HandleInput(input);
                CheckGameState();
            }
        }

        Console.WriteLine(_state == GameState.Enlightened
            ? "\nYou have found yourself. You are free."
            : "\nYou were lost to insanity...");
    }

    private void Setup()
    {
        var conscienceRoom = new Room("The Conscience Room", "A glowing white void. This is where you must return.");
        var liquorStore = new Room("The Liquor Store", "A flickering neon sign buzzes overhead. Smells like pain.");

        var flashbackToken = new FlashbackToken(
            "Flashback Token",
            "A token of memories. Use it to unlock the past.",
            _interludes
        );

        liquorStore.AddItem(flashbackToken);

        liquorStore.AddItem(new TruthFragment("You remember laughter and loss."));
        liquorStore.AddItem(new TruthFragment("A fragment of your mother's voice."));
        liquorStore.AddItem(new TruthFragment("A forgotten dream resurfaces."));

        liquorStore.AddConnection("north", new Connection(conscienceRoom));
        conscienceRoom.AddConnection("south", new Connection(liquorStore));
        
        _player = new Character(
            liquorStore,
            _intrusiveThoughtsManager,
            _interludes,
            () => _state = GameState.Enlightened
        );

        _player.CurrentRoom.Describe(_player);
    }
    
    private void HandleInput(string input)
    {
        if (_intrusiveThoughtsManager.CurrentEffect == IntrusiveThoughtEffect.FreezePlayer)
        {
            Console.WriteLine("You feel paralyzed by thought. You can’t act this turn.");
            _turnCount++;
            return;
        }
        
        var args = input.Split(" ", 2);
        var command = args[0].ToLower();

        switch (command)
        {
            case "go":
                if (args.Length > 1) _player.Move(args[1]);
                else Console.WriteLine("Go where?");
                break;

            case "take":
                if (args.Length > 1) _player.TakeItem(args[1]);
                else Console.WriteLine("Take what?");
                break;

            case "use":
                if (args.Length > 1)
                {
                    var itemName = args[1].ToLower();
                    var item = _player.Inventory.GetItem(itemName);
                    if (item != null)
                    {
                        item.Use(_player);
                    }
                    else
                    {
                        Console.WriteLine("You don't have that item.");
                    }
                }
                else
                {
                    Console.WriteLine("Use what?");
                }
                break;

            case "inventory":
                _player.Inventory.ListItems();
                break;

            case "help":
                Console.WriteLine("Commands: go [dir], take [item], use [item], inventory, help");
                break;

            default:
                Console.WriteLine("Unknown command. Try 'help'.");
                break;
        }

        _turnCount++;
        _interludes.CheckAndPlay(_turnCount);
        _intrusiveThoughtsManager.TriggerIntrusiveThoughts(_turnCount);
    }
    
    private void CheckGameState()
    {
        if (_player.CurrentRoom.Name == "The Conscience Room" &&
            _player.Inventory.CountTruthFragments() >= 1)
        {
            _state = GameState.Enlightened;
        }

        if (_turnCount >= MaxTurns)
        {
            _state = GameState.Consumed;
        }
    }
}