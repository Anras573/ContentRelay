namespace ContentDistributionMetadataProducer.Models;

public record ContentDistributionMetadata(
    string distributionDate,
    string[] distributionChannels,
    string[] distributionMethods,
    Assets[] assets
);