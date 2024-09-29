using ContentRelay.MAM.Application.Repositories;
using ContentRelay.MAM.Domain;
using ContentRelay.MAM.Web.Events.Ingoing;
using ContentRelay.MAM.Web.Mappers;
using ContentRelay.Shared;
using Microsoft.AspNetCore.Mvc;

namespace ContentRelay.MAM.Web.Endpoints;

public static class BriefingMetadataApiEndpoints
{
    public static void MapBriefingMetadataEndpoints(this WebApplication app)
    {
        app.MapPost("/briefingmetadata", (ILogger<BriefingEvent> logger,
                IBriefingRepository repository,
                [FromBody] List<BriefingEvent> briefingEvents) =>
            {
                logger.LogInformation("Received {Count} briefings", briefingEvents.Count);
                
                foreach (var briefing in briefingEvents)
                {
                    logger.LogInformation("Received briefing {@Briefing}", briefing);
                }
                
                var briefings = briefingEvents
                    .Select(briefing => briefing.ToDomain())
                    .ToList();
                
                if (briefings.Any(briefingOrError => !ValidateEvent(briefingOrError, logger)))
                {
                    return Results.BadRequest();
                }
                
                foreach (var briefing in briefings)
                {
                    briefing.Switch(repository.Add, _ => { /* Do nothing */ });
                }
                
                return Results.Accepted();
            })
            .WithTopic("pubsub", "briefing-metadata-topic")
            .WithName("PostBriefingMetadata")
            .WithOpenApi();
        
        return;
        
        bool ValidateEvent(OneOf<Briefing, ValidationErrors> briefingOrError, ILogger<BriefingEvent> logger)
        {
            return briefingOrError.Match(
                _ => true,
                errors =>
                {
                    foreach (var error in errors)
                    {
                        logger.LogError("Validation error: {Error}", error);
                    }

                    return false;
                });
        }
    }
}