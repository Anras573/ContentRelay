using System.Text.Json;
using AssetMetadataProducer.Models;
using ContentRelay.Shared;

namespace AssetMetadataProducer.Readers;

public class AssetMetadataReader
{
    private const string MetadataPath = "Metadata/AssetMetadata.json";
    
    public async Task<Maybe<List<AssetMetadata>>> ReadMetadataAsync()
    {
        var json = await File.ReadAllTextAsync(MetadataPath);
        if (string.IsNullOrWhiteSpace(json))
        {
            return Maybe<List<AssetMetadata>>.None();
        }
        
        var metadata = JsonSerializer.Deserialize<List<AssetMetadata>>(json);
        
        return metadata == null
            ? Maybe<List<AssetMetadata>>.None()
            : Maybe<List<AssetMetadata>>.Some(metadata);
    }
}