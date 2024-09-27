using ContentRelay.MAM.Web.Events.Ingoing;
using Microsoft.AspNetCore.Mvc;

namespace ContentRelay.MAM.Web.Endpoints;

public static class BriefingMetadataApiEndpoints
{
    public static void MapBriefingMetadataEndpoints(this WebApplication app)
    {
        app.MapPost("/briefingmetadata", (ILogger<BriefingEvent> logger, [FromBody] List<BriefingEvent> briefings) =>
            {
                logger.LogInformation("Received {Count} briefings", briefings.Count);
                
                foreach (var briefing in briefings)
                {
                    logger.LogInformation("Received briefing {@Briefing}", briefing);
                }

                
                return Results.Accepted();
            })
            .WithTopic("pubsub", "briefing-metadata-topic")
            .WithName("PostBriefingMetadata")
            .WithOpenApi();
    }
}