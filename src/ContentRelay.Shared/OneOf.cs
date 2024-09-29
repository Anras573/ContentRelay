namespace ContentRelay.Shared;

public class OneOf<T1, T2>
{
    private readonly T1? _value1;
    private readonly T2? _value2;
    
    private OneOf(T1 value)
    {
        _value1 = value;
    }
    
    private OneOf(T2 value)
    {
        _value2 = value;
    }
    
    public TOut Match<TOut>(Func<T1, TOut> first, Func<T2, TOut> second)
    {
        return _value1 != null
            ? first(_value1)
            : second(_value2!);
    }
    
    public void Switch(Action<T1> first, Action<T2> second)
    {
        if (_value1 != null)
        {
            first(_value1);
        }
        else
        {
            second(_value2!);
        }
    }
    
    public static implicit operator OneOf<T1, T2>(T1 value) => new(value);
    public static implicit operator OneOf<T1, T2>(T2 value) => new(value);
}