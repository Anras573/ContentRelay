using ContentRelay.MAM.Application.Mappers;
using ContentRelay.MAM.Application.Repositories;
using ContentRelay.MAM.Domain;
using ContentRelay.MAM.Web.Events.Ingoing;
using ContentRelay.MAM.Web.Mappers;
using ContentRelay.Shared;
using Microsoft.AspNetCore.Mvc;

namespace ContentRelay.MAM.Web.Endpoints;

public static class AssetMetadataApiEndpoints
{
    public static void MapAssetMetadataEndpoints(this WebApplication app)
    {
        app.MapPost("/assetmetadata", (ILogger<AssetEvent> logger, IAssetFileSizeCalculator assetFileSizeCalculator, IAssetRepository repository, [FromBody] List<AssetEvent> events) =>
            {
                logger.LogInformation("Received {Count} events", events.Count);
                
                foreach (var @event in events)
                {
                    logger.LogInformation("Received event {@Event}", @event);
                }

                var assets = events
                    .Select(@event => @event.ToDomain(assetFileSizeCalculator))
                    .ToList();

                if (assets.Any(assetOrError => !ValidateEvent(assetOrError, logger)))
                {
                    return Results.BadRequest();
                }
                
                foreach (var asset in assets)
                {
                    asset.Switch(repository.Add, _ => { /* Do nothing */ });
                }
                
                return Results.Accepted();
            })
            .WithTopic("pubsub", "asset-metadata-topic")
            .WithName("PostAssetMetadata")
            .WithOpenApi();
        
        app.MapGet("/assetmetadata/{id}", (ILogger<AssetEvent> logger, IAssetRepository repository, string id) =>
            {
                var assetIdOrError = AssetId.From(id);
                
                return assetIdOrError.Match(assetId =>
                {
                    var asset = repository.Get(assetId);
                    
                    return asset.Match(
                        Results.Ok,
                        () => Results.NotFound());
                }, Results.BadRequest);
            })
            .WithName("GetAssetMetadata")
            .WithOpenApi();
        
        return;

        bool ValidateEvent(OneOf<Asset, ValidationErrors> assetOrError, ILogger<AssetEvent> logger)
        {
            return assetOrError.Match(
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