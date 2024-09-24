using Dapr.Client;
using OrderListMetadataProducer.Models;

namespace OrderListMetadataProducer.Publishers;

public class OrderListMetadataPublisher
{
    private readonly DaprClient _client;
    private readonly ILogger<OrderListMetadataPublisher> _logger;
    private const string PubSubName = "pubsub";
    private const string TopicName = "briefing-metadata-topic";

    public OrderListMetadataPublisher(DaprClient client, ILogger<OrderListMetadataPublisher> logger)
    {
        _client = client;
        _logger = logger;
    }
    
    public async Task PublishMetadataAsync(OrderListMetadata metadata, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing metadata for order {OrderNumber}", metadata.orderNumber);

        try
        {
            await _client.PublishEventAsync(PubSubName, TopicName, metadata, cancellationToken);
            
            _logger.LogInformation("Successfully published metadata for order {OrderNumber}", metadata.orderNumber);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to publish metadata");
            throw;
        }
    }
}