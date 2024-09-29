using ContentRelay.MAM.Application.Mappers;
using ContentRelay.MAM.Infrastructure.Models;
using ContentRelay.Shared;

namespace ContentRelay.MAM.Infrastructure.Mappers;

public static class OrderListMapper
{
    public static OrderList ToInfrastructure(this Domain.OrderList orderList)
    {
        var briefs = orderList.Briefs.Select(ToInfrastructure).ToList();
        
        return new OrderList
        {
            OrderNumber = orderList.OrderNumber.Value,
            Briefs = briefs,
            CampaignName = orderList.CampaignName.Value,
            OrderDate = orderList.OrderDate.Value,
            RequesterName = orderList.RequesterName.Value
        };
    }
    
    private static Brief ToInfrastructure(this Domain.Brief brief)
    {
        return new Brief
        {
            BriefId = brief.Id.Value,
            Quantity = brief.Quantity
        };
    }
    
    public static OneOf<Domain.OrderList, ValidationErrors> ToDomain(this OrderList orderList)
    {
        var validationErrors = new ValidationErrors();
        
        var orderNumber = MapperHelper.ValidateField(orderList.OrderNumber, Domain.OrderNumber.From, nameof(OrderList.OrderNumber), validationErrors);
        var campaignName = MapperHelper.ValidateField(orderList.CampaignName, Domain.CampaignName.From, nameof(OrderList.CampaignName), validationErrors);
        var orderDate = MapperHelper.ValidateField(orderList.OrderDate, Domain.OrderDate.From, nameof(OrderList.OrderDate), validationErrors);
        var requesterName = MapperHelper.ValidateField(orderList.RequesterName, Domain.RequesterName.From, nameof(OrderList.RequesterName), validationErrors);
        
        var briefOrErrors = orderList.Briefs.Select(ToDomain).ToList();

        var briefs = briefOrErrors
            .Select(briefOrError => briefOrError.Match(
                brief => brief,
                errors =>
                {
                    foreach (var error in errors)
                    {
                        validationErrors.Add(error.Key, error.Value);
                    }
                    
                    return default!;
                }))
            .Select(brief => brief);
        
        if (validationErrors.Any)
        {
            return validationErrors;
        }
        
        var domainOrderList = new Domain.OrderList(
            orderNumber,
            requesterName,
            orderDate,
            campaignName
        );
        
        foreach (var brief in briefs)
        {
            domainOrderList.AddBrief(brief);
        }
        
        return domainOrderList;
    }
    
    private static OneOf<Domain.Brief, ValidationErrors> ToDomain(Brief brief)
    {
        var validationErrors = new ValidationErrors();
        
        var id = MapperHelper.ValidateField(brief.BriefId, Domain.BriefId.From, nameof(Brief.BriefId), validationErrors);
        
        if (validationErrors.Any)
        {
            return validationErrors;
        }
        
        return new Domain.Brief(id, brief.Quantity);
    }
}