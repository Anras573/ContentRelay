using ContentDistributionMetadataProducer.Models;
using Dapr.Client;

namespace ContentDistributionMetadataProducer.Publishers;

public class ContentDistributionMetadataPublisher
{
    private readonly DaprClient _client;
    private readonly ILogger<ContentDistributionMetadataPublisher> _logger;
    private const string PubSubName = "pubsub";
    private const string TopicName = "briefing-metadata-topic";

    public ContentDistributionMetadataPublisher(DaprClient client, ILogger<ContentDistributionMetadataPublisher> logger)
    {
        _client = client;
        _logger = logger;
    }
    
    public async Task PublishMetadataAsync(ContentDistributionMetadata metadata, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing metadata for Content Distribution by {DistributionDate}", metadata.distributionDate);

        try
        {
            await _client.PublishEventAsync(PubSubName, TopicName, metadata, cancellationToken);
            
            _logger.LogInformation("Successfully published metadata for Content Distribution by {DistributionDate}", metadata.distributionDate);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to publish metadata");
            throw;
        }
    }
}