using ContentRelay.MAM.Application.Mappers;
using ContentRelay.MAM.Domain;
using ContentRelay.MAM.Web.Events.Ingoing;
using ContentRelay.Shared;

namespace ContentRelay.MAM.Web.Mappers;

public static class ContentDistributionMapper
{
    public static OneOf<ContentDistribution, ValidationErrors> ToDomain(this ContentDistributionEvent cdEvent)
    {
        var validationErrors = new ValidationErrors();

        DistributionDate distributionDate;
        
        if (cdEvent.DistributionDate == "ToBeDefined")
        {
            distributionDate = DistributionDate.Empty;
        }
        else
        {
            distributionDate = MapperHelper.ValidateField(cdEvent.DistributionDate, DistributionDate.From,
                nameof(ContentDistributionEvent.DistributionDate), validationErrors);
        }
        
        var contentDistribution = new ContentDistribution(ContentDistributionId.NewId(), distributionDate);
        
        foreach (var asset in cdEvent.Assets)
        {
            var assetId = MapperHelper.ValidateField(asset.AssetId, AssetId.From, nameof(asset.AssetId), validationErrors);
            var assetName = MapperHelper.ValidateField(asset.Name, AssetName.From, nameof(asset.Name), validationErrors);
            
            contentDistribution.AddAsset(new Domain.ContentDistributionAsset(assetId, assetName, new Uri(asset.FileUrl)));
        }
        
        foreach (var channel in cdEvent.DistributionChannels)
        {
            var distributionChannel = MapperHelper.ValidateDistributionChannel(channel, validationErrors);
            contentDistribution.AddDistributionChannel(distributionChannel);
        }
        
        foreach (var method in cdEvent.DistributionMethods)
        {
            var distributionMethod = MapperHelper.ValidateDistributionMethod(method, validationErrors);
            contentDistribution.AddDistributionMethod(distributionMethod);
        }
        
        if (validationErrors.Any())
        {
            return validationErrors;
        }
        
        return contentDistribution;
    }
}