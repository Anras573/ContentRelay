using ContentRelay.MAM.Application.Mappers;
using ContentRelay.MAM.Domain;
using ContentRelay.MAM.Web.Events.Ingoing;
using ContentRelay.Shared;

namespace ContentRelay.MAM.Web.Mappers;

public static class OrderListMapper
{
    public static OneOf<OrderList, ValidationErrors> ToDomain(this OrderListEvent orderListEvent)
    {
        var validationErrors = new ValidationErrors();
        
        var orderNumber = MapperHelper.ValidateField(orderListEvent.OrderNumber, OrderNumber.From, nameof(OrderListEvent.OrderNumber), validationErrors);
        var requesterName = MapperHelper.ValidateField(orderListEvent.RequesterName, RequesterName.From, nameof(OrderListEvent.RequesterName), validationErrors);
        var orderDate = MapperHelper.ValidateField(orderListEvent.OrderDate, OrderDate.From, nameof(OrderListEvent.OrderDate), validationErrors);
        var campaignName = MapperHelper.ValidateField(orderListEvent.CampaignName, CampaignName.From, nameof(OrderListEvent.CampaignName), validationErrors);

        var briefs = orderListEvent.Briefs.Select(b =>
        {
            var briefId = MapperHelper.ValidateField(b.BriefId, BriefId.From, nameof(OrderListBrief.BriefId),
                validationErrors);
            var quantity = b.Quantity;

            return new Brief(briefId, quantity);
        });
        
        if (validationErrors.Any)
        {
            return validationErrors;
        }
        
        var orderList = new OrderList(orderNumber, requesterName, orderDate, campaignName);
        
        foreach (var brief in briefs)
        {
            orderList.AddBrief(brief);
        }
        
        return orderList;
    }
}