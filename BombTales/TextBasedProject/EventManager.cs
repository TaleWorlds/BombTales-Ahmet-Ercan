namespace TextBasedProject;
internal class EventManager
{
    private static EventManager _instance = null!;
    private readonly Action<StateManager.State> _onStateChanged;
    
    public EventManager() {
        _instance = Helper.GetSingleton(_instance, this);
        _onStateChanged += _ => { };
    }

    
    public void InvokeStateChanged(StateManager.State state) {
        if (!Helper.IsSingleton(_instance, this))
            return;
        
        _onStateChanged?.Invoke(state);
    }
}