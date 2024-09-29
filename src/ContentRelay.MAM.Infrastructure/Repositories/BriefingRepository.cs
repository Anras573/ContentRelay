using ContentRelay.MAM.Application.Repositories;
using ContentRelay.MAM.Domain;
using ContentRelay.MAM.Infrastructure.Mappers;
using ContentRelay.Shared;
using Microsoft.Extensions.Logging;
using Briefing = ContentRelay.MAM.Infrastructure.Models.Briefing;

namespace ContentRelay.MAM.Infrastructure.Repositories;

public class BriefingRepository : IBriefingRepository
{
    private readonly ILogger<BriefingRepository> _logger;
    private readonly List<Briefing> _briefings = [];

    public BriefingRepository(ILogger<BriefingRepository> logger)
    {
        _logger = logger;
    }
    
    public void Add(Domain.Briefing briefing)
    {
        _briefings.Add(briefing.ToInfrastructure());
    }
    
    public Maybe<Domain.Briefing> Get(Domain.BriefId briefId)
    {
        var briefing = _briefings.FirstOrDefault(b => b.Id == briefId.Value);
        
        if (briefing is null)
        {
            return Maybe<Domain.Briefing>.None;
        }
        
        var domainBriefing = briefing.ToDomain();

        return domainBriefing.Match(
            Maybe<Domain.Briefing>.Some,
            errors =>
            {
                foreach (var error in errors)
                {
                    _logger.LogError("Error converting briefing: {Key}: {Value}", error.Key, error.Value);
                }
                
                return Maybe<Domain.Briefing>.None;
            });
    }

    public Maybe<Domain.Briefing> GetByAssetId(AssetId assetId)
    {
        var briefing = _briefings.FirstOrDefault(b => b.AssetId == assetId.Value);
        
        if (briefing is null)
        {
            return Maybe<Domain.Briefing>.None;
        }
        
        var domainBriefing = briefing.ToDomain();

        return domainBriefing.Match(
            Maybe<Domain.Briefing>.Some,
            errors =>
            {
                foreach (var error in errors)
                {
                    _logger.LogError("Error converting briefing: {Key}: {Value}", error.Key, error.Value);
                }
                
                return Maybe<Domain.Briefing>.None;
            });
    }
}