using ContentRelay.Shared;

namespace ContentRelay.MAM.Application.Repositories;

public interface IBriefingRepository
{
    void Add(Domain.Briefing briefing);
    Maybe<Domain.Briefing> Get(Domain.BriefId briefingId);
    Maybe<Domain.Briefing> GetByAssetId(Domain.AssetId assetId);
}