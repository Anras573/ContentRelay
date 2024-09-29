namespace ContentRelay.Shared;

public class Maybe<T>
{
    private readonly T? _value;
    private bool HasValue => _value != null;

    private Maybe()
    {
        _value = default;
    }
    
    private Maybe(T value)
    {
        _value = value;
    }
    
    public TOut Match<TOut>(Func<T, TOut> some, Func<TOut> none)
    {
        return _value != null
            ? some(_value)
            : none();
    }
    
    public Task<TOut> MatchAsync<TOut>(Func<T, Task<TOut>> some, Func<Task<TOut>> none)
    {
        return _value != null
            ? some(_value)
            : none();
    }
    
    public void Switch(Action<T> some, Action none)
    {
        if (_value != null)
        {
            some(_value);
        }
        else
        {
            none();
        }
    }
    
    public async Task SwitchAsync(Func<T, Task> some, Func<Task> none)
    {
        if (_value != null)
        {
            await some(_value);
        }
        else
        {
            await none();
        }
    }
    
    public bool IsNone => !HasValue;
    public bool IsSome => HasValue;
    
    public static Maybe<T> Some(T value) => new (value);
    public static Maybe<T> None => new ();
    
    public static implicit operator Maybe<T>(T value) => Some(value);
}
