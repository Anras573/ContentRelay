using ContentRelay.MAM.Application.Repositories;
using ContentRelay.MAM.Domain;
using ContentRelay.MAM.Infrastructure.Mappers;
using ContentRelay.Shared;
using Microsoft.Extensions.Logging;
using ContentDistribution = ContentRelay.MAM.Infrastructure.Models.ContentDistribution;

namespace ContentRelay.MAM.Infrastructure.Repositories;

public class ContentDistributionRepository : IContentDistributionRepository
{
    private readonly ILogger<ContentDistributionRepository> _logger;
    private readonly List<ContentDistribution> _contentDistributions = [];

    public ContentDistributionRepository(ILogger<ContentDistributionRepository> logger)
    {
        _logger = logger;
    }

    public void Add(Domain.ContentDistribution contentDistribution)
    {
        _contentDistributions.Add(contentDistribution.ToInfrastructure());
    }

    public Maybe<Domain.ContentDistribution> Get(ContentDistributionId contentDistributionId)
    {
        var contentDistribution = _contentDistributions.FirstOrDefault(cd => cd.Id == contentDistributionId.Value.ToString());
        
        if (contentDistribution is null)
        {
            return Maybe<Domain.ContentDistribution>.None;
        }
        
        var domainContentDistribution = contentDistribution.ToDomain();

        return domainContentDistribution.Match(
            Maybe<Domain.ContentDistribution>.Some,
            errors =>
            {
                foreach (var error in errors)
                {
                    _logger.LogError("Error converting content distribution: {Key}: {Value}", error.Key, error.Value);
                }
                
                return Maybe<Domain.ContentDistribution>.None;
            });
    }

    public IReadOnlyList<Domain.ContentDistribution> GetAllUnpublished()
    {
        var unpublishedContentDistributions = _contentDistributions
            .Where(cd => !cd.HasBeenPublished)
            .Select(cd => cd.ToDomain())
            .ToList();
        
        List<ValidationErrors> validationErrors = [];
        List<Domain.ContentDistribution> domainContentDistributions = [];
        
        foreach (var contentDistribution in unpublishedContentDistributions)
        {
            contentDistribution.Switch(
                cd => domainContentDistributions.Add(cd),
                errors => validationErrors.Add(errors));
        }

        if (validationErrors.Count == 0)
            return domainContentDistributions;
        
        
        foreach (var error in validationErrors.SelectMany(errors => errors))
        {
            _logger.LogError("Error converting content distribution: {Key}: {Value}", error.Key, error.Value);
        }
        
        return ArraySegment<Domain.ContentDistribution>.Empty;
    }

    public void Update(Domain.ContentDistribution contentDistribution)
    {
        var existingContentDistribution = _contentDistributions.FirstOrDefault(cd => cd.Id == contentDistribution.Id.Value.ToString());
        
        if (existingContentDistribution is null)
        {
            _logger.LogWarning("Content distribution {ContentDistributionId} not found", contentDistribution.Id);
            return;
        }
        
        _contentDistributions.Remove(existingContentDistribution);
        _contentDistributions.Add(contentDistribution.ToInfrastructure());
    }
}