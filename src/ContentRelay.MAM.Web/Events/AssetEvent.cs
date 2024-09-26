namespace ContentRelay.MAM.Web.Events;

public record AssetEvent(
    string AssetId,
    string Name,
    string Description,
    string FileFormat,
    string FileSize,
    string Path,
    string CreatedBy,
    string VersionNumber,
    string UserName,
    string Comments,
    string Preview,
    string Status);