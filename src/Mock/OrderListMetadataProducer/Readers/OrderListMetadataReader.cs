using System.Text.Json;
using ContentRelay.Shared;
using OrderListMetadataProducer.Models;

namespace OrderListMetadataProducer.Readers;

public class OrderListMetadataReader
{
    private const string MetadataPath = "Metadata/OrderListMetadata.json";
    
    public async Task<Maybe<OrderListMetadata>> ReadMetadataAsync()
    {
        var json = await File.ReadAllTextAsync(MetadataPath);
        if (string.IsNullOrWhiteSpace(json))
        {
            return Maybe<OrderListMetadata>.None;
        }
        
        var metadata = JsonSerializer.Deserialize<OrderListMetadata>(json);
        
        return metadata == null
            ? Maybe<OrderListMetadata>.None
            : Maybe<OrderListMetadata>.Some(metadata);
    }
}