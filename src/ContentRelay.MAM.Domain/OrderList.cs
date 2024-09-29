namespace ContentRelay.MAM.Domain;

public record OrderList
{
    public OrderList(
        OrderNumber orderNumber,
        RequesterName requesterName,
        OrderDate orderDate,
        CampaignName campaignName)
    {
        OrderNumber = orderNumber;
        RequesterName = requesterName;
        OrderDate = orderDate;
        CampaignName = campaignName;
    }

    public int TotalBriefs => InnerBriefs.Sum(b => b.Quantity);

    public CampaignName CampaignName { get; }

    public OrderDate OrderDate { get; }

    public RequesterName RequesterName { get; }

    public OrderNumber OrderNumber { get; }
    
    private List<Brief> InnerBriefs { get; } = [];

    public IReadOnlyList<Brief> Briefs => InnerBriefs.AsReadOnly();
    
    public void AddBrief(Brief brief)
    {
        InnerBriefs.Add(brief);
    }
};