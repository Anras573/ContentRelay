using ContentRelay.MAM.Application.Builders;
using ContentRelay.MAM.Application.Repositories;
using ContentRelay.MAM.Domain;
using Microsoft.Extensions.Logging;

namespace ContentRelay.MAM.Application.Services;

public class MetadataService
{
    private readonly ILogger<MetadataService> _logger;
    private readonly IAssetRepository _assetRepository;
    private readonly IBriefingRepository _briefingRepository;
    private readonly IContentDistributionRepository _contentDistributionRepository;
    private readonly IOrderListRepository _orderListRepository;


    public MetadataService(
        ILogger<MetadataService> logger,
        IAssetRepository assetRepository,
        IBriefingRepository briefingRepository,
        IContentDistributionRepository contentDistributionRepository,
        IOrderListRepository orderListRepository)
    {
        _logger = logger;
        _assetRepository = assetRepository;
        _briefingRepository = briefingRepository;
        _contentDistributionRepository = contentDistributionRepository;
        _orderListRepository = orderListRepository;
    }
    
    public List<Metadata> GetMetadataToPublish()
    {
        List<Metadata> metadataList = [];
        
        var unpublishedContentDistributions = _contentDistributionRepository.GetAllUnpublished();
        
        foreach(var contentDistribution in unpublishedContentDistributions)
        {
            var shouldSkip = false;
            var assetsIds = contentDistribution.Assets.Select(asset => asset.Id).ToList();
            List<Asset> assets = [];

            foreach (var assetId in assetsIds)
            {
                _assetRepository.Get(assetId).Switch(asset => assets.Add(asset), () => shouldSkip = true);
            }
            
            if (shouldSkip)
            {
                _logger.LogWarning("Skipping content distribution {ContentDistributionId} because some assets are missing", contentDistribution.Id);
                continue;
            }
            
            List<Briefing> briefings = [];

            foreach (var assetId in assetsIds)
            {
                _briefingRepository.GetByAssetId(assetId).Switch(briefing => briefings.Add(briefing), () => shouldSkip = true);
            }
            
            if (shouldSkip)
            {
                _logger.LogWarning("Skipping content distribution {ContentDistributionId} because some briefings are missing", contentDistribution.Id);
                continue;
            }
            
            if (briefings.Count == 0)
            {
                _logger.LogWarning("Skipping content distribution {ContentDistributionId} because no briefings are associated with the assets", contentDistribution.Id);
                continue;
            }

            var orderList = _orderListRepository.GetByBriefId(briefings.First().Id).Match(
                orderList => orderList,
                () =>
                {
                    _logger.LogWarning("Skipping content distribution {ContentDistributionId} because no order list is associated with the briefing", contentDistribution.Id);
                    shouldSkip = true;
                    return default;
                });
            
            if (assets.Count != briefings.Count)
            {
                _logger.LogWarning("Skipping content distribution {ContentDistributionId} because the number of assets and briefings do not match", contentDistribution.Id);
                shouldSkip = true;
            }
            
            if (shouldSkip)
            {
                continue;
            }

            foreach (var asset in assets)
            {
                var briefing = briefings.First(b => b.AssetId == asset.Id);
                
                var metadata = new MetadataBuilder()
                    .WithAsset(asset)
                    .WithBriefing(briefing)
                    .WithOrderList(orderList)
                    .WithContentDistribution(contentDistribution)
                    .Build();
                
                metadataList.Add(metadata);
            }
            
        }
        
        return metadataList;
    }
    
    public void MarkAsPublished(Metadata metadata)
    {
        var contentDistribution = _contentDistributionRepository.Get(metadata.ContentDistributionId);
        
        contentDistribution.Switch(cd =>
        {
            cd.Publish();
            _contentDistributionRepository.Update(cd);
        }, () => { _logger.LogWarning("Content distribution {ContentDistributionId} not found", metadata.ContentDistributionId); });
    }
}