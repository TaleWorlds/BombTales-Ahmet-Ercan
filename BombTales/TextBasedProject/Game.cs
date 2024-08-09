using TextBasedProject;
using Timer = TextBasedProject.Timer;

public class Game
{
    private const string GameName = "BombTales!";
    private const int RoundTime = 300;
    private const int TimerReminderDuration = 30;

    #region Singleton
    private static Game _instance;
    public static Game Instance => _instance ??= new Game();
    #endregion

    private enum PublicFunctions
    {
        Start,
        StartWithoutIntroduction,
        StartGame,
        StartTutorial,
        TimeLeft,
        SkipTutorial,
        ExitGame,
        GetBomb,
        BreakTheGame,
        GetAvailableFunctions,
    }

    // Class Instances
    private readonly Timer _timer;
    private readonly TextManager _textManager;
    private readonly StateManager _stateManager;
    private EventManager _eventManager;
    private Bomb _bomb;
    private bool _classesInitialized;
    private bool _tutorialStarted;

    public Game()
    {
        CommonData.GameName = GameName;
        CommonData.GameTime = RoundTime;
        CommonData.TimerReminderDuration = TimerReminderDuration;
        
        // Debug.ActivateDebugging();
        
        _stateManager = new StateManager();
        _textManager = new TextManager();
        _timer = new Timer();
        _eventManager = new EventManager();
        
        _classesInitialized = true;
        _timer._onTimerEnd += GameEnded;
     
        _setFunctionsToStateManger();
        _setEventManagerReferences();
    }

    private void GameEnded()
    {
        _stateManager.ChangeState(StateManager.State.Start);
        var time = _timer.StopTimer(true);
        if (time != CommonData.GameTime) {
            Console.WriteLine("Oyunu " + time + " saniyede bitirdin. Tebrikler!");
        }
        _bomb.DisableBomb();
        _bomb = null;
    }

    public void Start()
    {
        _textManager.Welcoming();
        if (_isFunctionNotAvailable(PublicFunctions.Start.ToString())) return;

        _stateManager.ChangeState(StateManager.State.Introduction);
        _textManager.Introduction();

    }
    public void StartWithoutIntroduction()
    {
        if (_isFunctionNotAvailable(PublicFunctions.StartWithoutIntroduction.ToString())) return;
        _startFunction();
    }

    public void StartGame()
    {
        if (_isFunctionNotAvailable(PublicFunctions.StartGame.ToString())) return;

        if (_tutorialStarted)
        {
            _stateManager.RemoveFunctionFromState(StateManager.State.Introduction, PublicFunctions.StartGame.ToString());
            Console.WriteLine("Bomban oluşturuldu şimdi GetBomb() fonksiyonunu kullanarak bomba instance'ını alabilirsin.");
            Console.WriteLine("Örnek Instance alma kodu: var b = Game.Instance.GetBomb()");
            _stateManager.AddFunctionToState(StateManager.State.Introduction, PublicFunctions.GetBomb.ToString());
            _initializeBomb();
            return;
        }
        _startFunction();
    }

    public void ExitGame()
    {
        if (_isFunctionNotAvailable(PublicFunctions.ExitGame.ToString())) return;

        GameEnded();
        _textManager.Exit();
    }

    public void GetAvailableFunctions()
    {
        var functions = _stateManager.GetAvailableFunctions();
        Console.WriteLine("Available functions for the '" + _stateManager.CurrentState + "' state are: ");
        foreach (var function in functions) {
            Console.WriteLine(function);
        }
    }
    public void StartTutorial()
    {
        if (_isFunctionNotAvailable(PublicFunctions.StartTutorial.ToString())) return;

        _tutorialStarted = true;
        _stateManager.AddFunctionToState(StateManager.State.Introduction, PublicFunctions.StartGame.ToString());
        Console.WriteLine("Oyun başladığında sana bir bomba verilecek ve artık oyununu o instance üzerinden yönetebileceksin.");
        Console.WriteLine("Oyunu başlattıktan sonra bomba instance'ını alıp onu imha etmeye çalışacaksın");
        Console.WriteLine("Game üzerinden istediğin zaman oyunu sonlandırabilirsin. Ama unutma, oyunu sonlandırdığında kaybedersin. :)");
        Console.WriteLine("Haydi örnek bir oyun başlatalım. StartGame() fonksiyonunu kullanarak oyunu başlatmaya ne dersin?");
    }
    public void SkipTutorial()
    {
        if (_isFunctionNotAvailable(PublicFunctions.SkipTutorial.ToString())) return;
        _startFunction();
    }

    public void TimeLeft()
    {
        if (_isFunctionNotAvailable(PublicFunctions.TimeLeft.ToString())) return;
        Console.WriteLine("*-----------------*");
        Console.Write("Time left: ");
        _timer.GetTimeLeft();
        Console.WriteLine("*-----------------*");
    }

    public Bomb GetBomb() {
        if (_isFunctionNotAvailable(PublicFunctions.GetBomb.ToString())) return null;
            
        if (_bomb == null) _initializeBomb();
        if (_tutorialStarted)
        {
            Console.WriteLine("Tebrikler Bombanı aldın! Oyunun devamında bütün işlerini bu bomba ile yapıcaksın.");
            Console.WriteLine("Eğer ki Bombayı kaybedersen bu fonksiyonu kullanarak tekrardan erişebilirsin bombana");
            Console.WriteLine("Bombayı çözmen için " + CommonData.GameTime + " Saniye süren olacak.");
            Console.WriteLine("Bombanın da kullanabileceğin fonksiyonlarına bakmak için GetAvailableFunctions() fonksiyonunu kullanabilirsin");
            Console.WriteLine("Bombayı nasıl çözeceğin bombanın kitapçığında yazıyor.");
            Console.WriteLine("Tutorial'ı başarıyla bitirdin istersen elindeki oyuncak bombayı çözmeye çalışabilirsin, elinde patlamaz");
            Console.WriteLine("Artık yeni bir oyuna başlıyacağın zaman StartWithoutIntroduction() fonksiyonunu kullanabilirsin.");
            Console.WriteLine("Asıl oyuna geçmek için SkipTutorial() fonksiyonunu kullanabilirsin.");
            _tutorialStarted = false;
        }

        return _bomb;
    }
    public void BreakTheGame() {
        if (_isFunctionNotAvailable(PublicFunctions.BreakTheGame.ToString())) return;
        
        Console.WriteLine("Bu sadece heyecan yaratmak için yazdığımız bir fonksiyon! Buradan ekmek çıkmaz");
    }

    private bool _isFunctionNotAvailable(string functionName) {
        return !_stateManager.IsFunctionAvailable(functionName);
    }

    private void _startTimer()
    {
        Debug.Log("Timer has started with " + CommonData.GameTime + " Seconds.");
        _timer.StartTimer(CommonData.GameTime);
    }
    private void _setEventManagerReferences() {
        _stateManager.SetEventManager(_eventManager);
    }
    private void _setFunctionsToStateManger() 
    {
        // General Functions
        _stateManager.AddGeneralFunction([
            PublicFunctions.GetAvailableFunctions.ToString(),
            PublicFunctions.BreakTheGame.ToString()
        ]);
        
        // Start State
        _stateManager.AddFunctionToState(StateManager.State.Start, [
            PublicFunctions.Start.ToString(),
            PublicFunctions.StartWithoutIntroduction.ToString()
        ]);
        
        // Introduction State
        _stateManager.AddFunctionToState(StateManager.State.Introduction, [
            PublicFunctions.StartTutorial.ToString(),
            PublicFunctions.SkipTutorial.ToString()
        ]);
        
        // Game State
        _stateManager.AddFunctionToState(StateManager.State.Game, [
            PublicFunctions.ExitGame.ToString(),
            PublicFunctions.TimeLeft.ToString(),
            PublicFunctions.GetBomb.ToString()
        ]);
    }
    private void _initializeBomb() {
        if (_bomb != null)
        {
            Bomb._singleton = null!;
            _bomb.BombDetonated -= GameEnded;
            _bomb._gameEndedEvent -= GameEnded;
            _bomb = null!;
        }
        _bomb = new Bomb();
        _bomb.BombDetonated += GameEnded;
        _bomb._gameEndedEvent += GameEnded;
    }


    private void _startFunction()
    {
        _tutorialStarted = false;
        _initializeBomb();
        _startTimer();
        _stateManager.ChangeState(StateManager.State.Game);
        Console.WriteLine("Oyun Başladı!");
    }


    public override string ToString() {
        return CommonData.GameName;
    }
}