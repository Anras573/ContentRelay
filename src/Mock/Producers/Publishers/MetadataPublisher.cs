using Dapr.Client;
using Producers.Models;

namespace Producers.Publishers;

public class MetadataPublisher
{
    private readonly ILogger<MetadataPublisher> _logger;
    private readonly DaprClient _client;

    public MetadataPublisher(ILogger<MetadataPublisher> logger, DaprClient client)
    {
        _logger = logger;
        _client = client;
    }
    
    public async Task PublishMetadataAsync(
        List<AssetMetadata> metadata,
        string pubsubName,
        string topicName,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing metadata for {Type}", nameof(AssetMetadata));

        try
        {
            await _client.PublishEventAsync(pubsubName, topicName, metadata, cancellationToken);
            
            _logger.LogInformation("Successfully published metadata for {Type}", nameof(AssetMetadata));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to publish metadata");
            throw;
        }
    }
    
    public async Task PublishMetadataAsync(
        List<BriefingMetadata> metadata,
        string pubsubName,
        string topicName,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing metadata for {Type}", nameof(BriefingMetadata));

        try
        {
            await _client.PublishEventAsync(pubsubName, topicName, metadata, cancellationToken);
            
            _logger.LogInformation("Successfully published metadata for {Type}", nameof(BriefingMetadata));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to publish metadata");
            throw;
        }
    }
    
    public async Task PublishMetadataAsync(
        ContentDistributionMetadata metadata,
        string pubsubName,
        string topicName,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing metadata for {Type}", nameof(ContentDistributionMetadata));

        try
        {
            await _client.PublishEventAsync(pubsubName, topicName, metadata, cancellationToken);
            
            _logger.LogInformation("Successfully published metadata for {Type}", nameof(ContentDistributionMetadata));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to publish metadata");
            throw;
        }
    }
    
    public async Task PublishMetadataAsync(
        OrderListMetadata metadata,
        string pubsubName,
        string topicName,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing metadata for {Type}", nameof(OrderListMetadata));

        try
        {
            await _client.PublishEventAsync(pubsubName, topicName, metadata, cancellationToken);
            
            _logger.LogInformation("Successfully published metadata for {Type}", nameof(OrderListMetadata));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to publish metadata");
            throw;
        }
    }
}