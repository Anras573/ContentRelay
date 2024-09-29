using ContentRelay.Shared;

namespace ContentRelay.MAM.Application.Repositories;

public interface IOrderListRepository
{
    void Add(Domain.OrderList orderList);
    Maybe<Domain.OrderList> Get(Domain.OrderNumber orderListId);
    Maybe<Domain.OrderList> GetByBriefId(Domain.BriefId briefingId);
}