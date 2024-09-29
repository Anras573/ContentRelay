using ContentRelay.MAM.Application.Mappers;
using ContentRelay.MAM.Domain;
using ContentRelay.Shared;
using Asset = ContentRelay.MAM.Infrastructure.Models.Asset;

namespace ContentRelay.MAM.Infrastructure.Mappers;

public static class AssetMapper
{
    public static Asset ToInfrastructure(this Domain.Asset asset)
    {
        return new Asset
        {
            AssetId = asset.Id.Value,
            Name = asset.Name.Value,
            Description = asset.Description,
            FileFormat = asset.FileFormat.Value,
            FileSize = asset.FileSize,
            Path = asset.Path.ToString(),
            CreatedBy = asset.CreatedBy.Value,
            VersionNumber = asset.VersionNumber.Value,
            TimeStamp = asset.TimeStamp.Value,
            UserName = asset.UserName.Value,
            Comments = asset.Comments,
            Preview = asset.Preview.ToString(),
            Status = asset.AssetStatus.ToString()
        };
    }
    
    public static OneOf<Domain.Asset, ValidationErrors> ToDomain(this Asset asset)
    {
        var validationErrors = new ValidationErrors();
        
        var id = MapperHelper.ValidateField(asset.AssetId, AssetId.From, nameof(Asset.AssetId), validationErrors);
        var name = MapperHelper.ValidateField(asset.Name, AssetName.From, nameof(Asset.Name), validationErrors);
        var createdBy = MapperHelper.ValidateField(asset.CreatedBy, CreatedBy.From, nameof(Asset.CreatedBy), validationErrors);
        var format = MapperHelper.ValidateField(asset.FileFormat, FileFormat.From, nameof(Asset.FileFormat), validationErrors);
        var versionNumber = MapperHelper.ValidateField(asset.VersionNumber, VersionNumber.From, nameof(Asset.VersionNumber), validationErrors);
        var timeStamp = MapperHelper.ValidateField(asset.TimeStamp, TimeStamp.From, nameof(Asset.TimeStamp), validationErrors);
        var userName = MapperHelper.ValidateField(asset.UserName, UserName.From, nameof(Asset.UserName), validationErrors);
        
        var status = MapperHelper.ValidateAssetStatus(asset.Status, validationErrors);

        var path = new Uri(asset.Path);
        var preview = new Uri(asset.Preview);
        
        if (validationErrors.Any)
        {
            return validationErrors;
        }
        
        return new Domain.Asset(
            id,
            name,
            asset.Description,
            format,
            asset.FileSize,
            path,
            createdBy,
            versionNumber,
            timeStamp,
            userName,
            asset.Comments,
            preview,
            status
        );
    }
}