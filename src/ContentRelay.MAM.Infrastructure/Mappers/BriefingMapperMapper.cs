using ContentRelay.MAM.Application.Mappers;
using ContentRelay.MAM.Domain;
using ContentRelay.Shared;
using Briefing = ContentRelay.MAM.Infrastructure.Models.Briefing;

namespace ContentRelay.MAM.Infrastructure.Mappers;

public static class BriefingMapper
{
    public static Briefing ToInfrastructure(this Domain.Briefing briefing)
    {
        return new Briefing
        {
            Id = briefing.Id.Value,
            AssetId = briefing.AssetId.Value,
            Name = briefing.Name.Value,
            Description = briefing.Description,
            CreatedBy = briefing.CreatedBy.Value,
            CreatedDate = briefing.CreatedDate.Value,
            Comments = briefing.Comments,
            Status = briefing.BriefStatus.ToString()
        };
    }
    
    public static OneOf<Domain.Briefing, ValidationErrors> ToDomain(this Briefing briefing)
    {
        var validationErrors = new ValidationErrors();
        
        var id = MapperHelper.ValidateField(briefing.Id, BriefId.From, nameof(Briefing.Id), validationErrors);
        var assetId = MapperHelper.ValidateField(briefing.AssetId, AssetId.From, nameof(Briefing.AssetId), validationErrors);
        var name = MapperHelper.ValidateField(briefing.Name, BriefingName.From, nameof(Briefing.Name), validationErrors);
        var createdBy = MapperHelper.ValidateField(briefing.CreatedBy, CreatedBy.From, nameof(Briefing.CreatedBy), validationErrors);
        var createdDate = MapperHelper.ValidateField(briefing.CreatedDate, CreatedDate.From, nameof(Briefing.CreatedDate), validationErrors);
        
        var status = MapperHelper.ValidateBriefStatus(briefing.Status, validationErrors);
        
        if (validationErrors.Any)
        {
            return validationErrors;
        }
        
        return new Domain.Briefing(
            id,
            name,
            briefing.Description,
            assetId,
            createdBy,
            createdDate,
            status,
            briefing.Comments
        );
    }
}