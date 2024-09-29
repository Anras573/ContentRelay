namespace ContentRelay.MAM.Web.Events.Ingoing;

public record OrderListEvent(
    string OrderNumber,
    string RequesterName,
    string OrderDate,
    string CampaignName,
    int TotalBriefs,
    OrderListBrief[] Briefs);
    
public record OrderListBrief(string BriefId, int Quantity);
