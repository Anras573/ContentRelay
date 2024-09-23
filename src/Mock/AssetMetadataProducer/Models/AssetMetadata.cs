namespace AssetMetadataProducer.Models;

public record AssetMetadata(
    string assetId,
    string name,
    string description,
    string fileFormat,
    string fileSize,
    string path,
    string createdBy,
    string VersionNumber,
    string Timestamp,
    string UserName,
    string Comments,
    string Preview,
    string Status
);

