using ContentRelay.MAM.Web.Events.Ingoing;
using Microsoft.AspNetCore.Mvc;

namespace ContentRelay.MAM.Web.Endpoints;

public static class OrderListMetadataApiEndpoints
{
    public static void MapOrderListMetadataEndpoints(this WebApplication app)
    {
        app.MapPost("/orderlistmetadata", (ILogger<OrderListEvent> logger, [FromBody] OrderListEvent orderList) =>
            {
                logger.LogInformation("Received order list with {Length} briefs", orderList.Briefs.Length);
                
                logger.LogInformation("Received order list {orderList}", orderList);
                
                foreach (var brief in orderList.Briefs)
                {
                    logger.LogInformation("Received brief {@Brief}", brief);
                }
                
                return Results.Accepted();
            })
            .WithTopic("pubsub", "order-list-metadata-topic")
            .WithName("PostOrderListMetadata")
            .WithOpenApi();
    }
}