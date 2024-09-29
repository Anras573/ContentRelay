using ContentRelay.Shared;

namespace ContentRelay.MAM.Domain;

public record AssetName : ValueObject
{
    public static AssetName Empty => new("ToBeDefined");
    public string Value { get; }
    
    private AssetName(string value)
    {
        Value = value;
    }
    
    public static OneOf<AssetName, ValidationError> From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return AssetNameError.InvalidName;
        }
        
        return new AssetName(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public record AssetNameError(string Message) : ValidationError(Message)
{
    public static AssetNameError InvalidName => new("Asset name must not be empty");
}