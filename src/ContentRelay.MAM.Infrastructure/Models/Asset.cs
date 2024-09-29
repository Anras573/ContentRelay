namespace ContentRelay.MAM.Infrastructure.Models;

public class Asset
{
    public string AssetId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string FileFormat { get; set; }
    public long FileSize { get; set; }
    public string Path { get; set; }
    public string CreatedBy { get; set; }
    public string VersionNumber { get; set; }
    public DateTimeOffset TimeStamp { get; set; }
    public string UserName { get; set; }
    public string Comments { get; set; }
    public string Preview { get; set; }
    public string Status { get; set; }
}