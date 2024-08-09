namespace TextBasedProject;

internal class Debug
{
    private static bool _isDebugging;

    public static void Log(string message)
    {
        if (_isDebugging)
            Console.WriteLine(message);
    }
    public static void ActivateDebugging(){
        _isDebugging = true;
    }
}