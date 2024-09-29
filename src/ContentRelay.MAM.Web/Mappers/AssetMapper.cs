using ContentRelay.MAM.Application.Mappers;
using ContentRelay.MAM.Domain;
using ContentRelay.MAM.Web.Events.Ingoing;
using ContentRelay.Shared;

namespace ContentRelay.MAM.Web.Mappers;

public static class AssetMapper
{
    public static OneOf<Asset, ValidationErrors> ToDomain(this AssetEvent assetEvent, IAssetFileSizeCalculator assetFileSizeCalculator)
    {
        var validationErrors = new ValidationErrors();
        
        // Since format is not supplied properly by the asset event, we need to fetch it from the path
        var fileFormat = assetEvent.Path.Split('.').Last();
        var format = MapperHelper.ValidateField(fileFormat, FileFormat.From, nameof(AssetEvent.FileFormat), validationErrors);
        
        var id = MapperHelper.ValidateField(assetEvent.AssetId, AssetId.From, nameof(AssetEvent.AssetId), validationErrors);
        var name = MapperHelper.ValidateField(assetEvent.Name, AssetName.From, nameof(AssetEvent.Name), validationErrors);
        var createdBy = MapperHelper.ValidateField(assetEvent.CreatedBy, CreatedBy.From, nameof(AssetEvent.CreatedBy), validationErrors);
        var versionNumber = MapperHelper.ValidateField(assetEvent.VersionNumber, VersionNumber.From, nameof(AssetEvent.VersionNumber), validationErrors);
        var timeStamp = MapperHelper.ValidateField(assetEvent.TimeStamp, TimeStamp.From, nameof(AssetEvent.TimeStamp), validationErrors);
        var userName = MapperHelper.ValidateField(assetEvent.UserName, UserName.From, nameof(AssetEvent.UserName), validationErrors);
        
        var fileSize = MapperHelper.ValidateFileSize(assetEvent.Path, assetFileSizeCalculator, validationErrors);

        var status = MapperHelper.ValidateAssetStatus(assetEvent.Status, validationErrors);

        var path = new Uri(assetEvent.Path);
        var preview = new Uri(assetEvent.Preview);
        
        if (validationErrors.Any)
        {
            return validationErrors;
        }
        
        return new Asset(
            id,
            name,
            assetEvent.Description,
            format,
            fileSize,
            path,
            createdBy,
            versionNumber,
            timeStamp,
            userName,
            assetEvent.Comments,
            preview,
            status
        );
    }
}