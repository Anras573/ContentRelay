namespace ContentRelay.Shared;

public class ValidationErrors
{
    private readonly Dictionary<string, string> _errors = new();
    
    public void Add(string key, string errorMessage)
    {
        _errors[key] = errorMessage;
    }
    
    public bool Any => _errors.Count != 0;
}