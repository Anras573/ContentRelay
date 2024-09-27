namespace ContentRelay.MAM.Web.Events.Ingoing;

public record BriefingEvent(
    string Name,
    string Description,
    string AssetId,
    string CreatedBy,
    string CreatedDate,
    string Status,
    string Comments
);