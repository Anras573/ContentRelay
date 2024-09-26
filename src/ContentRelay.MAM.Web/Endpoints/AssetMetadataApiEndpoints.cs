using ContentRelay.MAM.Web.Events;
using Microsoft.AspNetCore.Mvc;

namespace ContentRelay.MAM.Web.Endpoints;

public static class AssetMetadataApiEndpoints
{
    public static void MapAssetMetadataEndpoints(this WebApplication app)
    {
        app.MapPost("/assetmetadata", (ILogger<AssetEvent> logger, [FromBody] List<AssetEvent> @events) =>
            {
                logger.LogInformation("Received {Count} events", @events.Count);
                
                foreach (var @event in @events)
                {
                    logger.LogInformation("Received event {@Event}", @event);
                }

                
                return Results.Accepted();
            })
            .WithTopic("pubsub", "asset-metadata-topic")
            .WithName("PostAssetMetadata")
            .WithOpenApi();
    }
}