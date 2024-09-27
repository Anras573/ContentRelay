namespace ContentRelay.MAM.Web.Events.Ingoing;

public record ContentDistributionEvent(
    string DistributionDate,
    string[] DistributionChannels,
    string[] DistributionMethods,
    ContentDistributionAsset[] Assets);
    
public record ContentDistributionAsset(
    string AssetId,
    string Name,
    string FileUrl);