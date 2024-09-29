using ContentRelay.Shared;

namespace ContentRelay.MAM.Domain;

public record RequesterName : ValueObject
{
    public static RequesterName Empty => new("ToBeDefined");
    public string Value { get; }
    
    private RequesterName(string value)
    {
        Value = value;
    }
    
    public static OneOf<RequesterName, ValidationError> From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return RequesterNameError.InvalidRequesterName;
        }
        
        return new RequesterName(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public record RequesterNameError(string Message) : ValidationError(Message)
{
    public static RequesterNameError InvalidRequesterName => new("Requester name must be a valid string");
}