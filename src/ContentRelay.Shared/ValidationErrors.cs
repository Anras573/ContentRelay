using System.Collections;

namespace ContentRelay.Shared;

public class ValidationErrors : IEnumerable<KeyValuePair<string, string>>
{
    private readonly Dictionary<string, string> _errors = new();
    
    public void Add(string key, string errorMessage)
    {
        _errors[key] = errorMessage;
    }
    
    public bool Any => _errors.Count != 0;
    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return _errors.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}