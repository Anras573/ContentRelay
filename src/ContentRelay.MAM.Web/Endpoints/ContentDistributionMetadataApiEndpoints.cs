using ContentRelay.MAM.Web.Events.Ingoing;
using Microsoft.AspNetCore.Mvc;

namespace ContentRelay.MAM.Web.Endpoints;

public static class ContentDistributionMetadataApiEndpoints
{
    public static void MapContentDistributionMetadataEndpoints(this WebApplication app)
    {
        app.MapPost("/contentdistributionmetadata", (ILogger<ContentDistributionEvent> logger,
            [FromBody] ContentDistributionEvent contentDistribution) =>
        {
            logger.LogInformation("Received content distribution with {Length} assets",
                contentDistribution.Assets.Length);

            logger.LogInformation("Received content distribution {contentDistribution}", contentDistribution);
            var channels = "[" + string.Join(", ", contentDistribution.DistributionChannels) + "]";
            var methods = "[" + string.Join(", ", contentDistribution.DistributionMethods) + "]";
            logger.LogInformation("Received content distribution via channels: {channels}, and methods: {methods}", channels, methods);
                
            foreach (var asset in contentDistribution.Assets)
            {
                logger.LogInformation("Received asset {@Asset}", asset);
            }
                
            return Results.Accepted();
        })
        .WithTopic("pubsub", "content-distribution-metadata-topic")
        .WithName("PostContentDistributionMetadata")
        .WithOpenApi();
    }
}