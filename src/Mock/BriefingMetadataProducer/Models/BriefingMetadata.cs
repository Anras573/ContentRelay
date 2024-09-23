namespace BriefingMetadataProducer.Models;

public record BriefingMetadata(
    string name,
    string description,
    string assetId,
    string createdBy,
    string createdDate,
    string status,
    string comments
);