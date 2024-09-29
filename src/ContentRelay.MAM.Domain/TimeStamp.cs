using ContentRelay.Shared;

namespace ContentRelay.MAM.Domain;

public record TimeStamp : ValueObject
{
    public static TimeStamp Empty => new(DateTimeOffset.MinValue);
    public DateTimeOffset Value { get; }

    private TimeStamp(DateTimeOffset value)
    {
        Value = value;
    }
    
    public static OneOf<TimeStamp, ValidationError> From(string value)
    {
        if (!DateTimeOffset.TryParse(value, out var timestamp))
        {
            return TimeStampError.InvalidTimeStamp(value);
        }
        
        return new TimeStamp(timestamp);
    }
    
    public static OneOf<TimeStamp, ValidationError> From(DateTimeOffset value)
    {
        // This function should only be called from Infrastructure, meaning we have validated the timestamp already,
        // so we never return an error here.
        
        return new TimeStamp(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public record TimeStampError(string Message) : ValidationError(Message)
{
    public static TimeStampError InvalidTimeStamp(string timestamp) => new($"Invalid timestamp: {timestamp}");
}