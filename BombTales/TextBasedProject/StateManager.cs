namespace TextBasedProject;

internal class StateManager
{
    public enum State
    {
        Start,
        Introduction,
        Game,
    }
    
    internal State CurrentState { get; private set; } = State.Start;

    private readonly Dictionary<State, List<string>> _stateFunctions;
    private EventManager _eventManager = null!;
    private static StateManager _instance = null!;

    public StateManager()
    {
        _instance = Helper.GetSingleton(_instance, this);
        
        _stateFunctions = new Dictionary<State, List<string>>();
        foreach (State state in Enum.GetValues(typeof(State)))
        {
            _stateFunctions[state] = new List<string>();
        }
        Debug.Log("Initialized StateManager");
    }

    public bool IsFunctionAvailable(string functionName)
    {
        if (!Helper.IsSingleton(_instance, this))
            return false;
        
        if (_stateFunctions.TryGetValue(CurrentState, out var functions))
        {
            var isAvailable = functions.Contains(functionName);
            if (!isAvailable) {
                Console.WriteLine("StateManager:\nO kadar hızlı değil! Bu fonksiyonu şu an kullanamazsın. (Yardım: GetAvailableFunctions() kullanmayı dene.)");
                Debug.Log("Function is not available in this state. " + functionName + " is not available in " + CurrentState);
            }
            return isAvailable;
        }
        
        return false;
    }
    
    public void AddFunctionToState(State state, params string[] functions)
    {
        if (!Helper.IsSingleton(_instance, this))
            return;
        
        if (!_stateFunctions.ContainsKey(state))
        {
            _stateFunctions[state] = new List<string>();
        }

        foreach (var function in functions)
        {
            if (!_stateFunctions[state].Contains(function))
            {
                _stateFunctions[state].Add(function);
            }
        }
    }
    public void RemoveFunctionFromState(State state, params string[] functions)
    {
        if (!Helper.IsSingleton(_instance, this))
            return;
        
        if (!_stateFunctions.ContainsKey(state))
        {
            _stateFunctions[state] = new List<string>();
        }

        foreach (var function in functions)
        {
            if (_stateFunctions[state].Contains(function))
            {
                _stateFunctions[state].Remove(function);
            }
        }
    }

    public void AddGeneralFunction(string[] functions)
    {
        if (!Helper.IsSingleton(_instance, this))
            return;

        AddFunctionToState(State.Start, functions);
        AddFunctionToState(State.Introduction, functions);
        AddFunctionToState(State.Game, functions);
    }
    
    public void ChangeState(State newState)
    {
        if (!Helper.IsSingleton(_instance, this))
            return;

        CurrentState = newState;
        _eventManager.InvokeStateChanged(newState);
    }
    public void NextState() {
        if (!Helper.IsSingleton(_instance, this))
            return;

        
        CurrentState++;
        _eventManager.InvokeStateChanged(CurrentState);
    }

    public string[] GetAvailableFunctions()
    {
        if (!Helper.IsSingleton(_instance, this))
            return new string[]{};

        if (_stateFunctions.TryGetValue(CurrentState, out var functions))
        {
            return functions.ToArray();
        }
        return Array.Empty<string>();
    }

    public void SetEventManager(EventManager eventManager) {
        if (!Helper.IsSingleton(_instance, this))
            return;
        
        _eventManager = eventManager;
    }
}
