using ContentRelay.Shared;

namespace ContentRelay.MAM.Domain;

public record ContentDistributionId : ValueObject
{
    public Guid Value { get; }

    private ContentDistributionId(Guid value)
    {
        Value = value;
    }

    public static ContentDistributionId NewId() => new(Guid.NewGuid());
    
    public static OneOf<ContentDistributionId, ValidationError> From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return ContentDistributionIdError.Empty;
        }

        if (!Guid.TryParse(value, out var id))
        {
            return ContentDistributionIdError.InvalidFormat;
        }

        return new ContentDistributionId(id);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public record ContentDistributionIdError(string Message) : ValidationError(Message)
{
    public static ContentDistributionIdError Empty => new("Content distribution ID cannot be empty");
    public static ContentDistributionIdError InvalidFormat => new("Invalid content distribution ID format");
}