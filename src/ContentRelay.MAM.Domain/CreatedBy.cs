using ContentRelay.Shared;

namespace ContentRelay.MAM.Domain;

public record CreatedBy : ValueObject
{
    public static CreatedBy Empty => new("ToBeDefined");
    public string Value { get; }
    
    private CreatedBy(string value)
    {
        Value = value;
    }
    
    public static OneOf<CreatedBy, ValidationError> From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return CreatedByError.InvalidName;
        }
        
        return new CreatedBy(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public record CreatedByError(string Message) : ValidationError(Message)
{
    public static CreatedByError InvalidName => new("Created by must not be empty");
}