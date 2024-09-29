using ContentRelay.MAM.Application.Services;
using ContentRelay.MAM.Domain;
using Dapr.Client;

namespace ContentRelay.MAM.Web.Endpoints;

public static class MetadataEndpoints
{
    public static void MapMetadataEndpoints(this WebApplication app)
    {
        app.MapPost("/metadata", async (ILogger<Metadata> logger, DaprClient client, MetadataService service) =>
        {
            logger.LogInformation("Publishing metadata");
            
            var metadataToPublish = service.GetMetadataToPublish();

            var tasks = metadataToPublish.Select(async metadata =>
            {
                await client.PublishEventAsync("pubsub", "metadata-topic", metadata);
                logger.LogInformation("Published metadata {@Metadata}", metadata);
            });

            await Task.WhenAll(tasks);

            foreach (var metadata in metadataToPublish)
            {
                service.MarkAsPublished(metadata);
            }

            return Results.Accepted();
        });
    }
}