using ContentRelay.MAM.Application.Repositories;
using ContentRelay.MAM.Domain;
using ContentRelay.MAM.Web.Events.Ingoing;
using ContentRelay.MAM.Web.Mappers;
using ContentRelay.Shared;
using Microsoft.AspNetCore.Mvc;

namespace ContentRelay.MAM.Web.Endpoints;

public static class ContentDistributionMetadataApiEndpoints
{
    public static void MapContentDistributionMetadataEndpoints(this WebApplication app)
    {
        app.MapPost("/contentdistributionmetadata", (ILogger<ContentDistributionEvent> logger,
            IContentDistributionRepository repository,
            [FromBody] ContentDistributionEvent contentDistribution) =>
        {
            logger.LogInformation("Received content distribution with {Length} assets",
                contentDistribution.Assets.Length);

            logger.LogInformation("Received content distribution {ContentDistribution}", contentDistribution);
            var channels = "[" + string.Join(", ", contentDistribution.DistributionChannels) + "]";
            var methods = "[" + string.Join(", ", contentDistribution.DistributionMethods) + "]";
            logger.LogInformation("Received content distribution via channels: {Channels}, and methods: {Methods}", channels, methods);
                
            foreach (var asset in contentDistribution.Assets)
            {
                logger.LogInformation("Received asset {@Asset}", asset);
            }
            
            var distribution = contentDistribution.ToDomain();

            if (!ValidateEvent(distribution, logger))
            {
                return Results.BadRequest();
            }
            
            distribution.Switch(repository.Add, _ => { /* Do nothing */ });
            
            return Results.Accepted();
        })
        .WithTopic("pubsub", "content-distribution-metadata-topic")
        .WithName("PostContentDistributionMetadata")
        .WithOpenApi();
        
        return;
        
        bool ValidateEvent(OneOf<ContentDistribution, ValidationErrors> contentDistributionOrError, ILogger<ContentDistributionEvent> logger)
        {
            return contentDistributionOrError.Match(
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