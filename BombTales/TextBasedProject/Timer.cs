namespace TextBasedProject;
internal class Timer
{
    private static Timer _instance;


    private string _debugString = "Time: ";
    private float _time;
    private int _gameTime;
    private bool _timerStarted;
    private DateTime _startTime;
    private CancellationTokenSource _cancellationTokenSource;
    private Game _game;
    internal Action _onTimerEnd;

    public Timer(){
        _instance = Helper.GetSingleton(_instance, this);
    }

    public void SetGame(Game game){
        if (!Helper.IsSingleton(_instance, this)) return;

        _game = game;
    }
    
    public void StartTimer(int gameTime)
    {
        if (!Helper.IsSingleton(_instance, this))
            return;
        _debugString = "";
        _gameTime = gameTime;
        _timerStarted = true;
        _startTime = DateTime.Now;
        _cancellationTokenSource = new CancellationTokenSource();
        _ = _runTimerAsync(_cancellationTokenSource.Token);
    }
    public void GetTimeLeft()
    {
        if (!Helper.IsSingleton(_instance, this))
            return;

        Console.WriteLine($"{_gameTime - (int)_time} seconds");
    }
    public int StopTimer(bool gameTimeNeeded = false)
    {
        if (!Helper.IsSingleton(_instance, this))
            return 0;
        _cancellationTokenSource.Cancel();
        Debug.Log("Timer stopped.");
        if (gameTimeNeeded)
        {
            return (int)(DateTime.Now - _startTime).TotalSeconds;
        }
        return 0;
    }

    
    private async Task _runTimerAsync(CancellationToken cancellationToken) {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(50); // Delay for 200 milliseconds
            _timerCallback();
        }
    }
    private void _timerCallback()
    {
        if (!_timerStarted) return;

        _time = (float)(DateTime.Now - _startTime).TotalSeconds;
        if ((int) _time % CommonData.TimerReminderDuration == 0)
        {
            var remainingTime = _gameTime - (int)_time;
            var result = "Time: " + remainingTime;
            if (result != _debugString){
                _debugString = result;
                Debug.Log(_debugString);
                Console.WriteLine(_debugString);
            }
        }
        if (_time >= _gameTime)
        {
            Debug.Log("Game Over");
            Console.WriteLine("Oyunu Kaybettiniz!");
            _onTimerEnd?.Invoke();
            _cancellationTokenSource.Cancel(); // Cancel the timer
        }
    }
}

