using System.Text.Json;
using BriefingMetadataProducer.Models;
using ContentRelay.Shared;

namespace BriefingMetadataProducer.Readers;

public class BriefingMetadataReader
{
    private const string MetadataPath = "Metadata/BriefingMetadata.json";
    
    public async Task<Maybe<List<BriefingMetadata>>> ReadMetadataAsync()
    {
        var json = await File.ReadAllTextAsync(MetadataPath);
        if (string.IsNullOrWhiteSpace(json))
        {
            return Maybe<List<BriefingMetadata>>.None();
        }
        
        var metadata = JsonSerializer.Deserialize<List<BriefingMetadata>>(json);
        
        return metadata == null
            ? Maybe<List<BriefingMetadata>>.None()
            : Maybe<List<BriefingMetadata>>.Some(metadata);
    }
}