using ContentRelay.MAM.Application.Repositories;
using ContentRelay.MAM.Domain;
using ContentRelay.MAM.Infrastructure.Mappers;
using ContentRelay.Shared;
using Microsoft.Extensions.Logging;
using OrderList = ContentRelay.MAM.Infrastructure.Models.OrderList;

namespace ContentRelay.MAM.Infrastructure.Repositories;

public class OrderListRepository : IOrderListRepository
{
    private readonly ILogger<OrderListRepository> _logger;
    private readonly List<OrderList> _orderLists = [];

    public OrderListRepository(ILogger<OrderListRepository> logger)
    {
        _logger = logger;
    }

    public void Add(Domain.OrderList orderList)
    {
        _orderLists.Add(orderList.ToInfrastructure());
    }

    public Maybe<Domain.OrderList> Get(Domain.OrderNumber orderNumber)
    {
        var orderList = _orderLists.FirstOrDefault(o => o.OrderNumber == orderNumber.Value);
        
        if (orderList is null)
        {
            return Maybe<Domain.OrderList>.None;
        }
        
        var domainOrderList = orderList.ToDomain();

        return domainOrderList.Match(
            Maybe<Domain.OrderList>.Some,
            errors =>
            {
                foreach (var error in errors)
                {
                    _logger.LogError("Error converting order list: {Key}: {Value}", error.Key, error.Value);
                }
                
                return Maybe<Domain.OrderList>.None;
            });
    }

    public Maybe<Domain.OrderList> GetByBriefId(BriefId briefingId)
    {
        var orderList = _orderLists
            .FirstOrDefault(o => o.Briefs.FirstOrDefault(b => b.BriefId == briefingId.Value) is not null);
        
        if (orderList is null)
        {
            return Maybe<Domain.OrderList>.None;
        }
        
        var domainOrderList = orderList.ToDomain();

        return domainOrderList.Match(
            Maybe<Domain.OrderList>.Some,
            errors =>
            {
                foreach (var error in errors)
                {
                    _logger.LogError("Error converting order list: {Key}: {Value}", error.Key, error.Value);
                }
                
                return Maybe<Domain.OrderList>.None;
            });
    }
}