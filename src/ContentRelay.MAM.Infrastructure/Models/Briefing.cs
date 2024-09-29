namespace ContentRelay.MAM.Infrastructure.Models;

public class Briefing
{
    public string Id { get; set; } = "";
    public string AssetId { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string CreatedBy { get; set; } = "";
    public DateTime CreatedDate { get; set; }
    public string Status { get; set; } = "";
    public string Comments { get; set; } = "";
}