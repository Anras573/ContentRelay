namespace Producers.Models;

public record OrderListMetadata(
    string orderNumber,
    string requesterName,
    string orderDate,
    string campaignName,
    int totalBriefs,
    Briefs[] briefs
);