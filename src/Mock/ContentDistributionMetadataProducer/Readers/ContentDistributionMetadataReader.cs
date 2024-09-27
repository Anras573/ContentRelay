using System.Text.Json;
using ContentDistributionMetadataProducer.Models;
using ContentRelay.Shared;

namespace ContentDistributionMetadataProducer.Readers;

public class ContentDistributionMetadataReader
{
    private const string MetadataPath = "Metadata/ContentDistributionMetadata.json";
        
    public async Task<Maybe<ContentDistributionMetadata>> ReadMetadataAsync()
    {
        var json = await File.ReadAllTextAsync(MetadataPath);
        if (string.IsNullOrWhiteSpace(json))
        {
            return Maybe<ContentDistributionMetadata>.None;
        }
        
        var metadata = JsonSerializer.Deserialize<ContentDistributionMetadata>(json);
        
        return metadata == null
            ? Maybe<ContentDistributionMetadata>.None
            : Maybe<ContentDistributionMetadata>.Some(metadata);
    }
}