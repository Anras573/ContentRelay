namespace ContentRelay.MAM.Infrastructure.Models;

public class ContentDistribution
{
    public string Id { get; set; } = "";
    
    public DateTime DistributionDate { get; set; }
    public List<ContentDistributionAsset> Assets { get; set; } = [];
    public List<string> DistributionChannels { get; set; } = [];
    public List<string> DistributionMethods { get; set; } = [];

    public bool HasBeenPublished { get; set; }
}