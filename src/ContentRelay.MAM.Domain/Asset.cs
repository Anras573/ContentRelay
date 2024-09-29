namespace ContentRelay.MAM.Domain;

public record Asset(
    AssetId Id,
    AssetName Name,
    string Description,
    FileFormat FileFormat,
    long FileSize,
    Uri Path,
    CreatedBy CreatedBy,
    VersionNumber VersionNumber,
    TimeStamp TimeStamp,
    UserName UserName,
    string Comments,
    Uri Preview,
    AssetStatus AssetStatus);