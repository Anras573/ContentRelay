using ContentRelay.MAM.Application.Mappers;
using ContentRelay.MAM.Infrastructure.Models;
using ContentRelay.Shared;

namespace ContentRelay.MAM.Infrastructure.Mappers;

public static class ContentDistributionMapper
{
    public static ContentDistribution ToInfrastructure(this Domain.ContentDistribution contentDistribution)
    {
        return new ContentDistribution
        {
            Id = contentDistribution.Id.Value.ToString(),
            DistributionDate = contentDistribution.DistributionDate.Value,
            Assets = contentDistribution.Assets.Select(a => new ContentDistributionAsset
            {
                Id = a.Id.Value,
                Name = a.Name.Value,
                FileUrl = a.FileUrl.ToString()
            }).ToList(),
            DistributionChannels = contentDistribution.DistributionChannels.Select(dc => dc.ToString()).ToList(),
            DistributionMethods = contentDistribution.DistributionMethods.Select(dm => dm.ToString()).ToList(),
            HasBeenPublished = contentDistribution.HasBeenPublished
        };
    }
    
    public static OneOf<Domain.ContentDistribution, ValidationErrors> ToDomain(this ContentDistribution contentDistribution)
    {
        var validationErrors = new ValidationErrors();
        
        var id = MapperHelper.ValidateField(contentDistribution.Id, Domain.ContentDistributionId.From, nameof(ContentDistribution.Id), validationErrors);
        var distributionDate = MapperHelper.ValidateField(contentDistribution.DistributionDate, Domain.DistributionDate.From, nameof(ContentDistribution.DistributionDate), validationErrors);
        var assetsOrErrors = contentDistribution.Assets.Select(ToDomain).ToList();
        var distributionChannels = contentDistribution.DistributionChannels.Select(dc => MapperHelper.ValidateDistributionChannel(dc, validationErrors)).ToList();
        var distributionMethods = contentDistribution.DistributionMethods.Select(dm => MapperHelper.ValidateDistributionMethod(dm, validationErrors)).ToList();
        
        var assets = assetsOrErrors
            .Select(assetOrError => assetOrError.Match(
                asset => asset,
                errors =>
                {
                    foreach (var error in errors)
                    {
                        validationErrors.Add(error.Key, error.Value);
                    }
                    
                    return default!;
                }))
            .Select(asset => asset);

        if (validationErrors.Any)
        {
            return validationErrors;
        }
        
        var cd = new Domain.ContentDistribution(
            id,
            distributionDate,
            contentDistribution.HasBeenPublished);
        
        foreach (var asset in assets)
        {
            cd.AddAsset(asset);
        }
        
        foreach (var distributionChannel in distributionChannels)
        {
            cd.AddDistributionChannel(distributionChannel);
        }
        
        foreach (var distributionMethod in distributionMethods)
        {
            cd.AddDistributionMethod(distributionMethod);
        }
        
        return cd;
    }
    
    private static OneOf<Domain.ContentDistributionAsset, ValidationErrors> ToDomain(ContentDistributionAsset contentDistributionAsset)
    {
        var validationErrors = new ValidationErrors();
        
        var id = MapperHelper.ValidateField(contentDistributionAsset.Id, Domain.AssetId.From, nameof(ContentDistributionAsset.Id), validationErrors);
        var name = MapperHelper.ValidateField(contentDistributionAsset.Name, Domain.AssetName.From, nameof(ContentDistributionAsset.Name), validationErrors);
        
        if (validationErrors.Any)
        {
            return validationErrors;
        }
        
        return new Domain.ContentDistributionAsset(
            id,
            name,
            new Uri(contentDistributionAsset.FileUrl));
    }
}