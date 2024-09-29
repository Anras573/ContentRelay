namespace ContentRelay.MAM.Infrastructure.Models;

public class OrderList
{
    public string OrderNumber { get; set; }
    public string RequesterName { get; set; }
    public DateTime OrderDate { get; set; }
    public string CampaignName { get; set; }

    public List<Brief> Briefs { get; set; }
}