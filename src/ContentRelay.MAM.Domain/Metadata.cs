namespace ContentRelay.MAM.Domain;

public record Metadata(
    AssetId AssetId,
    AssetName AssetName,
    string Description,
    FileFormat FileFormat,
    long FileSize,
    Uri Path,
    CreatedBy CreatedBy,
    VersionNumber VersionNumber,
    TimeStamp TimeStamp,
    UserName UserName,
    string AssetComments,
    Uri Preview,
    AssetStatus AssetStatus,
    
    BriefId BriefId,
    BriefingName BriefingName,
    string BriefingDescription,
    CreatedBy BriefingCreatedBy,
    CreatedDate BriefingCreatedDate,
    BriefStatus BriefingStatus,
    string BriefingComments,
    
    OrderNumber OrderNumber,
    OrderDate OrderDate,
    RequesterName RequesterName,
    CampaignName CampaignName,
    int Quantity,
    
    ContentDistributionId ContentDistributionId,
    DistributionDate DistributionDate,
    IReadOnlyList<DistributionChannel> DistributionChannels,
    IReadOnlyList<DistributionMethod> DistributionMethods,
    Uri FileUrl
    );