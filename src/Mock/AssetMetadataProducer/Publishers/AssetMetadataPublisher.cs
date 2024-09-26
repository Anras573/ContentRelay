using AssetMetadataProducer.Models;
using Dapr.Client;

namespace AssetMetadataProducer.Publishers;

public class AssetMetadataPublisher
{
    private readonly DaprClient _client;
    private readonly ILogger<AssetMetadataPublisher> _logger;
    private const string PubSubName = "pubsub";
    private const string TopicName = "asset-metadata-topic";

    public AssetMetadataPublisher(DaprClient client, ILogger<AssetMetadataPublisher> logger)
    {
        _client = client;
        _logger = logger;
    }
    
    public async Task PublishMetadataAsync(List<AssetMetadata> metadata, CancellationToken cancellationToken)
    {
        if (metadata.Count == 0)
        {
            _logger.LogWarning("No metadata provided to publish");
            return;
        }
        
        _logger.LogInformation("Publishing {Count} metadata items", metadata.Count);

        try
        {
            await _client.PublishEventAsync(PubSubName, TopicName, metadata, cancellationToken);
            
            _logger.LogInformation("Successfully published metadata for {Count} assets", metadata.Count);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to publish metadata");
            throw;
        }
    }
}
