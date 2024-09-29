using ContentRelay.MAM.Application.Repositories;
using ContentRelay.MAM.Domain;
using ContentRelay.MAM.Infrastructure.Mappers;
using ContentRelay.Shared;
using Microsoft.Extensions.Logging;
using Asset = ContentRelay.MAM.Infrastructure.Models.Asset;

namespace ContentRelay.MAM.Infrastructure.Repositories;

public class AssetRepository : IAssetRepository
{
    private readonly ILogger<AssetRepository> _logger;
    private readonly List<Asset> _assets = [];

    public AssetRepository(ILogger<AssetRepository> logger)
    {
        _logger = logger;
    }
    
    public void Add(Domain.Asset asset)
    {
        _assets.Add(asset.ToInfrastructure());
    }
    
    public Maybe<Domain.Asset> Get(AssetId assetId)
    {
        var asset = _assets.FirstOrDefault(a => a.AssetId == assetId.Value);
        
        if (asset is null)
        {
            return Maybe<Domain.Asset>.None;
        }
        
        var domainAsset = asset.ToDomain();

        return domainAsset.Match(
            Maybe<Domain.Asset>.Some,
            errors =>
            {
                foreach (var error in errors)
                {
                    _logger.LogError("Error converting asset: {Key}: {Value}", error.Key, error.Value);
                }
                
                return Maybe<Domain.Asset>.None;
            });
    }
}