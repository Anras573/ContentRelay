namespace ContentRelay.Shared;

public class Maybe<T>
{
    private readonly T? _value;

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
    
    public static Maybe<T> Some(T value) => new (value);
    public static Maybe<T> None() => new ();
    
    public static implicit operator Maybe<T>(T value) => Some(value);
}
