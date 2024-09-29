using ContentRelay.Shared;

namespace ContentRelay.MAM.Domain;

public record CreatedDate : ValueObject
{
    public static CreatedDate Empty => new (DateTime.MinValue);
    
    public DateTime Value { get; }
    
    private CreatedDate(DateTime value)
    {
        Value = value;
    }
    
    public static OneOf<CreatedDate, ValidationError> From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return CreatedDateError.Empty;
        }
        
        if (!DateTime.TryParse(value, out var date))
        {
            return CreatedDateError.InvalidFormat;
        }

        return new CreatedDate(date);
    }
    
    public static OneOf<CreatedDate, ValidationError> From(DateTime value)
    {
        // This function should only be called from Infrastructure, meaning we have validated the timestamp already,
        // so we never return an error here.
        
        return new CreatedDate(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public record CreatedDateError(string Message) : ValidationError(Message)
{
    public static CreatedDateError Empty => new ("Created date cannot be empty");
    public static CreatedDateError InvalidFormat => new ("Invalid date format");
}