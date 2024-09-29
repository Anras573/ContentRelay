using ContentRelay.Shared;

namespace ContentRelay.MAM.Domain;

public record CampaignName : ValueObject
{
    public static CampaignName Empty => new("ToBeDefined");
    public string Value { get; }
    
    private CampaignName(string value)
    {
        Value = value;
    }
    
    public static OneOf<CampaignName, ValidationError> From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return CampaignNameError.InvalidCampaignName;
        }
        
        return new CampaignName(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public record CampaignNameError(string Message) : ValidationError(Message)
{
    public static CampaignNameError InvalidCampaignName => new("Campaign name must be a valid string");
}