using ContentRelay.Shared;

namespace ContentRelay.MAM.Domain;

public record OrderDate : ValueObject
{
    public static OrderDate Empty => new(DateTime.MinValue);
    public DateTime Value { get; }
    
    private OrderDate(DateTime value)
    {
        Value = value;
    }
    
    public static OneOf<OrderDate, ValidationError> From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return OrderDateError.Empty;
        }
        
        if (!DateTime.TryParse(value, out var date))
        {
            return OrderDateError.InvalidFormat;
        }

        return new OrderDate(date);
    }
    
    public static OneOf<OrderDate, ValidationError> From(DateTime value)
    {
        // This function should only be called from Infrastructure, meaning we have validated the timestamp already,
        // so we never return an error here.
        
        return new OrderDate(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public record OrderDateError(string Message) : ValidationError(Message)
{
    public static OrderDateError Empty => new ("Order date cannot be empty");
    public static OrderDateError InvalidFormat => new ("Invalid date format");
}