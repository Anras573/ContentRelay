using System.Text.RegularExpressions;
using ContentRelay.Shared;

namespace ContentRelay.MAM.Domain;

public partial record AssetId
{
    public static AssetId Empty => new("EMPTY000");
    public string Value { get; }
    
    private AssetId(string id)
    {
        Value = id;
    }

    public static OneOf<AssetId, ValidationError> From(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return AssetIdError.InvalidAssetId;
        }

        var value = id.ToUpper();
        
        // Asset Id must be 8 characters long (ASSET001)
        if (value.Length != 8)
        {
            return AssetIdError.InvalidAssetId;
        }
        
        return AssetIdRegex().IsMatch(value) 
            ? new AssetId(id)
            : AssetIdError.InvalidAssetId;
    }

    [GeneratedRegex("(ASSET)\\d{3}")]
    private static partial Regex AssetIdRegex();
}

public record AssetIdError(string Message) : ValidationError(Message)
{
    public static AssetIdError InvalidAssetId => new("Asset id must be in the format 'ASSET001'");
}