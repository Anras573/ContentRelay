using ContentRelay.Shared;

namespace ContentRelay.MAM.Domain;

public record DistributionDate : ValueObject
{
    public static DistributionDate Empty => new (DateTime.MinValue);
    
    public DateTime Value { get; }

    private DistributionDate(DateTime value)
    {
        Value = value;
    }

    public static OneOf<DistributionDate, ValidationError> From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return DistributionDateError.Empty;
        }
        
        if (!DateTime.TryParse(value, out var date))
        {
            return DistributionDateError.InvalidFormat(value);
        }
        
        return new DistributionDate(date);
    }
    
    public static OneOf<DistributionDate, ValidationError> From(DateTime value)
    {
        // This function should only be called from Infrastructure, meaning we have validated the timestamp already,
        // so we never return an error here.
        
        return new DistributionDate(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public record DistributionDateError(string Message) : ValidationError(Message)
{
    public static DistributionDateError Empty => new ("Distribution date cannot be empty");
    public static DistributionDateError InvalidFormat(string value) => new ($"Invalid date format: {value}");
}
