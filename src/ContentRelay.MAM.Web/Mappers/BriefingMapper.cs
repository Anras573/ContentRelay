using ContentRelay.MAM.Application.Mappers;
using ContentRelay.MAM.Domain;
using ContentRelay.MAM.Web.Events.Ingoing;
using ContentRelay.Shared;

namespace ContentRelay.MAM.Web.Mappers;

public static class BriefingMapper
{
    public static OneOf<Briefing, ValidationErrors> ToDomain(this BriefingEvent briefingEvent)
    {
        var validationErrors = new ValidationErrors();
        
        // Brief Id is derived from the Asset Id
        var normalizedBriefId = briefingEvent
            .AssetId
            .ToUpper()
            .Replace("ASSET", "BRIEF");
        var briefId = MapperHelper.ValidateField(normalizedBriefId, BriefId.From, nameof(BriefId), validationErrors);
        
        var name = MapperHelper.ValidateField(briefingEvent.Name, BriefingName.From, nameof(BriefingEvent.Name), validationErrors);
        var assetId = MapperHelper.ValidateField(briefingEvent.AssetId, AssetId.From, nameof(BriefingEvent.AssetId), validationErrors);
        var createdBy = MapperHelper.ValidateField(briefingEvent.CreatedBy, CreatedBy.From, nameof(BriefingEvent.CreatedBy), validationErrors);
        var createdDate = MapperHelper.ValidateField(briefingEvent.CreatedDate, CreatedDate.From, nameof(BriefingEvent.CreatedDate), validationErrors);
        
        var status = MapperHelper.ValidateBriefStatus(briefingEvent.Status, validationErrors);
        
        return new Briefing(
            briefId,
            name,
            briefingEvent.Description,
            assetId,
            createdBy,
            createdDate,
            status,
            briefingEvent.Comments
        );
    }
}