using ContentRelay.MAM.Application.Repositories;
using ContentRelay.MAM.Domain;
using ContentRelay.MAM.Web.Events.Ingoing;
using ContentRelay.MAM.Web.Mappers;
using ContentRelay.Shared;
using Microsoft.AspNetCore.Mvc;

namespace ContentRelay.MAM.Web.Endpoints;

public static class OrderListMetadataApiEndpoints
{
    public static void MapOrderListMetadataEndpoints(this WebApplication app)
    {
        app.MapPost("/orderlistmetadata", (ILogger<OrderListEvent> logger,
                IOrderListRepository repository,
                [FromBody] OrderListEvent orderListEvent) =>
            {
                logger.LogInformation("Received order list with {Length} briefs", orderListEvent.Briefs.Length);
                
                logger.LogInformation("Received order list {OrderList}", orderListEvent);
                
                foreach (var brief in orderListEvent.Briefs)
                {
                    logger.LogInformation("Received brief {@Brief}", brief);
                }
                
                var orderList = orderListEvent.ToDomain();
                
                if (!ValidateEvent(orderList, logger))
                {
                    return Results.BadRequest();
                }
                
                orderList.Switch(repository.Add, _ => { /* Do nothing */ });
                
                return Results.Accepted();
            })
            .WithTopic("pubsub", "order-list-metadata-topic")
            .WithName("PostOrderListMetadata")
            .WithOpenApi();
        
        return;
        
        bool ValidateEvent(OneOf<OrderList, ValidationErrors> orderListOrError, ILogger<OrderListEvent> logger)
        {
            return orderListOrError.Match(
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