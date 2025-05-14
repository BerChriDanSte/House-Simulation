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
        
        var childhoodHome = new Room("Childhood Bedroom", "Dust dances in the sunlight. Everything is smaller than you remember."); 
        var liquorStore = new Room("The Liquor Store", "A flickering neon sign buzzes overhead. Smells like pain."); 
        var darkHallway = new Room("Dark Hallway", "You can't see anything. The air is thick and still.", isDark: true); 
        var therapistOffice = new Room("Therapist's Office", "Books line the shelves. A voice once told you to forgive yourself here."); 
        var cemetery = new Room("Forgotten Cemetery", "Wind brushes over cracked stones. Some of the names are missing."); 
        var alleyway = new Room("Back Alley", "Faint echoes of shouting and silence. Something happened here.");
        
        var flashlight = new Flashlight(); 
        liquorStore.AddItem(flashlight);
        
        var flashbackToken = new FlashbackToken("Flashback Token", "A token of memories. Use it to unlock the past.", _interludes); 
        cemetery.AddItem(flashbackToken);
        
        childhoodHome.AddItem(new TruthFragment("A broken crayon. A memory of innocence.")); 
        therapistOffice.AddItem(new TruthFragment("A conversation that shook your denial.")); 
        cemetery.AddItem(new TruthFragment("You wept here. Someone mattered.")); 
        alleyway.AddItem(new TruthFragment("The sound of footsteps and sirens. Then silence."));
        
        liquorStore.AddConnection("east", new Connection(darkHallway)); 
        darkHallway.AddConnection("west", new Connection(liquorStore));
        
        darkHallway.AddConnection("north", new Connection(childhoodHome)); 
        darkHallway.AddConnection("south", new Connection(therapistOffice)); 
        therapistOffice.AddConnection("north", new Connection(darkHallway)); 
        childhoodHome.AddConnection("south", new Connection(darkHallway));
        
        childhoodHome.AddConnection("east", new Connection(cemetery)); 
        cemetery.AddConnection("west", new Connection(childhoodHome));
        
        cemetery.AddConnection("south", new Connection(alleyway)); 
        alleyway.AddConnection("north", new Connection(cemetery));
        
        alleyway.AddConnection("east", new Connection(conscienceRoom, isLocked: true, requiredItemName: "Flashback Token")); 
        conscienceRoom.AddConnection("west", new Connection(alleyway));
        
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
        }
        else 
        { 
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
                
                case "look": 
                    _player.CurrentRoom.Describe(_player); 
                    break;
                
                case "help": 
                    Console.WriteLine("Commands: go [dir], take [item], use [item], inventory, look, help"); 
                    break;
                
                default: 
                    Console.WriteLine("Unknown command. Try 'help'."); 
                    break; 
            } 
        }
        
        _turnCount++; 
        _intrusiveThoughtsManager.TickEffect(); 
        _interludes.CheckAndPlay(_turnCount); 
        _intrusiveThoughtsManager.TriggerIntrusiveThoughts(_turnCount); 
    }
    
    private void CheckGameState()
    {
        if (_player.CurrentRoom.Name == "The Conscience Room")
        {
            if (_player.Inventory.CountTruthFragments() >= 3)
            {
                _state = GameState.Enlightened;
            }
            else
            {
                Console.WriteLine("You feel you're close... but you're missing pieces of yourself.");
            }
        }

        if (_turnCount >= MaxTurns)
        {
            _state = GameState.Consumed;
        }
    }
}