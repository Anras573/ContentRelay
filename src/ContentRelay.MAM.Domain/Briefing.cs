namespace ContentRelay.MAM.Domain;

public record Briefing(
    BriefId Id,
    BriefingName Name,
    string Description,
    AssetId AssetId,
    CreatedBy CreatedBy,
    CreatedDate CreatedDate,
    BriefStatus BriefStatus,
    string Comments);