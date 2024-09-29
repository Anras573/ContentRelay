namespace ContentRelay.MAM.Domain;

public record ContentDistribution(ContentDistributionId Id, DistributionDate DistributionDate, bool HasBeenPublished = false)
{
    public bool HasBeenPublished { get; private set; } = HasBeenPublished;
    
    private List<ContentDistributionAsset> InnerAssets { get; } = [];
    public IReadOnlyList<ContentDistributionAsset> Assets => InnerAssets.AsReadOnly();
    
    public void AddAsset(ContentDistributionAsset contentDistributionAsset)
    {
        InnerAssets.Add(contentDistributionAsset);
    }
    
    private List<DistributionChannel> InnerDistributionChannels { get; } = [];
    public IReadOnlyList<DistributionChannel> DistributionChannels => InnerDistributionChannels.AsReadOnly();
    
    public void AddDistributionChannel(DistributionChannel distributionChannel)
    {
        InnerDistributionChannels.Add(distributionChannel);
    }
    
    private List<DistributionMethod> InnerDistributionMethods { get; } = [];
    public IReadOnlyList<DistributionMethod> DistributionMethods => InnerDistributionMethods.AsReadOnly();
    
    public void AddDistributionMethod(DistributionMethod distributionMethod)
    {
        InnerDistributionMethods.Add(distributionMethod);
    }
    
    public void Publish()
    {
        HasBeenPublished = true;
    }
}
