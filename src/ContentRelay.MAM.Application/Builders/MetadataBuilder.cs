using ContentRelay.MAM.Domain;

namespace ContentRelay.MAM.Application.Builders;

public class MetadataBuilder :
    IMetadataAssetBuilder,
    IMetadataBriefingBuilder,
    IMetadataOrderListBuilder,
    IMetadataContentDistributionBuilder,
    IMetadataBuilder
{
    private Asset _asset;
    private Briefing _briefing;
    private ContentDistribution _contentDistribution;
    private OrderList _orderList;
    
    public IMetadataBriefingBuilder WithAsset(Asset asset)
    {
        _asset = asset;
        return this;
    }

    public IMetadataOrderListBuilder WithBriefing(Briefing briefing)
    {
        _briefing = briefing;
        return this;
    }

    public IMetadataContentDistributionBuilder WithOrderList(OrderList orderList)
    {
        _orderList = orderList;
        return this;
    }

    public IMetadataBuilder WithContentDistribution(ContentDistribution contentDistribution)
    {
        _contentDistribution = contentDistribution;
        return this;
    }

    public Metadata Build()
    {
        var quantity = _orderList.Briefs.First(b => b.Id == _briefing.Id).Quantity;
        var fileUrl = _contentDistribution.Assets.First(a => a.Id == _asset.Id).FileUrl;
        
        return new Metadata(
            // Asset
            _asset.Id,
            _asset.Name,
            _asset.Description,
            _asset.FileFormat,
            _asset.FileSize,
            _asset.Path,
            _asset.CreatedBy,
            _asset.VersionNumber,
            _asset.TimeStamp,
            _asset.UserName,
            _asset.Comments,
            _asset.Preview,
            _asset.AssetStatus,
            
            // Briefing
            _briefing.Id,
            _briefing.Name,
            _briefing.Description,
            _briefing.CreatedBy,
            _briefing.CreatedDate,
            _briefing.BriefStatus,
            _briefing.Comments,
            
            // OrderList
            _orderList.OrderNumber,
            _orderList.OrderDate,
            _orderList.RequesterName,
            _orderList.CampaignName,
            quantity,
            
            // ContentDistribution
            _contentDistribution.Id,
            _contentDistribution.DistributionDate,
            _contentDistribution.DistributionChannels,
            _contentDistribution.DistributionMethods,
            fileUrl
            );
    }
}

public interface IMetadataAssetBuilder
{
    IMetadataBriefingBuilder WithAsset(Asset asset);
}

public interface IMetadataBriefingBuilder
{
    IMetadataOrderListBuilder WithBriefing(Briefing briefing);
}

public interface IMetadataOrderListBuilder
{
    IMetadataContentDistributionBuilder WithOrderList(OrderList orderList);
}

public interface IMetadataContentDistributionBuilder
{
    IMetadataBuilder WithContentDistribution(ContentDistribution contentDistribution);
}

public interface IMetadataBuilder
{
    Metadata Build();
}