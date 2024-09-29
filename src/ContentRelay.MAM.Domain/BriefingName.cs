using ContentRelay.Shared;

namespace ContentRelay.MAM.Domain;

public record BriefingName : ValueObject
{
    public static BriefingName Empty => new ("ToBeDefined");
    
    public string Value { get; }
    
    private BriefingName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Briefing name cannot be empty");
        }

        Value = value;
    }
    
    public static OneOf<BriefingName, ValidationError> From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return BriefingNameError.Empty;
        }

        return new BriefingName(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public record BriefingNameError(string Message) : ValidationError(Message)
{
    public static BriefingNameError Empty => new ("Briefing name cannot be empty");
}