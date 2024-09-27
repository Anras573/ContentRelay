using ContentRelay.MAM.Domain;
using ContentRelay.MAM.Web.Events.Ingoing;
using ContentRelay.Shared;

namespace ContentRelay.MAM.Web.Mappers;

public static class AssetMapper
{
    public static OneOf<Asset, ValidationErrors> ToDomain(this AssetEvent assetEvent)
    {
        var validationErrors = new ValidationErrors();
        AssetId? id = default;

        var maybeId = AssetId.From(assetEvent.AssetId);
        maybeId.Switch(some => id = some, error => validationErrors.Add(nameof(AssetId), error.Message));
        
        if (validationErrors.Any)
        {
            return validationErrors;
        }
        
        return new Asset(
            id!
        );
    }
}