using System.Text.Json;
using ContentRelay.Shared;
using Producers.Models;

namespace Producers.Readers;

public class MetadataReader
{
    public async Task<Maybe<List<AssetMetadata>>> ReadAssetMetadataAsync(string path)
    {
        var json = await File.ReadAllTextAsync(path);
        if (string.IsNullOrWhiteSpace(json))
        {
            return Maybe<List<AssetMetadata>>.None;
        }
        
        var metadata = JsonSerializer.Deserialize<List<AssetMetadata>>(json);
        
        return metadata == null
            ? Maybe<List<AssetMetadata>>.None
            : Maybe<List<AssetMetadata>>.Some(metadata);
    }
    
    public async Task<Maybe<List<BriefingMetadata>>> ReadBriefingMetadataAsync(string path)
    {
        var json = await File.ReadAllTextAsync(path);
        if (string.IsNullOrWhiteSpace(json))
        {
            return Maybe<List<BriefingMetadata>>.None;
        }
        
        var metadata = JsonSerializer.Deserialize<List<BriefingMetadata>>(json);
        
        return metadata == null
            ? Maybe<List<BriefingMetadata>>.None
            : Maybe<List<BriefingMetadata>>.Some(metadata);
    }
    
    public async Task<Maybe<ContentDistributionMetadata>> ReadContentDistributionMetadataAsync(string path)
    {
        var json = await File.ReadAllTextAsync(path);
        if (string.IsNullOrWhiteSpace(json))
        {
            return Maybe<ContentDistributionMetadata>.None;
        }
        
        var metadata = JsonSerializer.Deserialize<ContentDistributionMetadata>(json);
        
        return metadata == null
            ? Maybe<ContentDistributionMetadata>.None
            : Maybe<ContentDistributionMetadata>.Some(metadata);
    }
    
    public async Task<Maybe<OrderListMetadata>> ReadOrderListMetadataAsync(string path)
    {
        var json = await File.ReadAllTextAsync(path);
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