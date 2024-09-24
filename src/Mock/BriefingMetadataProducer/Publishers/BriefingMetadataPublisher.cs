﻿using BriefingMetadataProducer.Models;
using Dapr.Client;

namespace BriefingMetadataProducer.Publishers;

public class BriefingMetadataPublisher
{
    private readonly DaprClient _client;
    private readonly ILogger<BriefingMetadataPublisher> _logger;
    private const string PubSubName = "pubsub";
    private const string TopicName = "briefing-metadata-topic";

    public BriefingMetadataPublisher(DaprClient client, ILogger<BriefingMetadataPublisher> logger)
    {
        _client = client;
        _logger = logger;
    }
    
    public async Task PublishMetadataAsync(List<BriefingMetadata> metadata, CancellationToken cancellationToken)
    {
        if (metadata.Count == 0)
        {
            _logger.LogWarning("No metadata provided to publish");
            return;
        }
        
        _logger.LogInformation("Publishing {Count} metadata items", metadata.Count);

        try
        {
            var publishTasks = metadata
                .Select(data => _client
                    .PublishEventAsync(PubSubName, TopicName, data, cancellationToken));
            
            await Task.WhenAll(publishTasks);
            
            _logger.LogInformation("Successfully published metadata for {Count} assets", metadata.Count);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to publish metadata");
            throw;
        }
    }
}