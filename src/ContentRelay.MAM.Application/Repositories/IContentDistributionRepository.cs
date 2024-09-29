using ContentRelay.MAM.Domain;
using ContentRelay.Shared;

namespace ContentRelay.MAM.Application.Repositories;

public interface IContentDistributionRepository
{
    void Add(ContentDistribution contentDistribution);
    
    Maybe<ContentDistribution> Get(ContentDistributionId contentDistributionId);
    
    IReadOnlyList<ContentDistribution> GetAllUnpublished();
    
    void Update(ContentDistribution contentDistribution);
}