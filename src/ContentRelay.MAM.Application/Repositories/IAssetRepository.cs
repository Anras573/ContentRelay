using ContentRelay.MAM.Domain;
using ContentRelay.Shared;

namespace ContentRelay.MAM.Application.Repositories;

public interface IAssetRepository
{
    void Add(Asset asset);
    Maybe<Asset> Get(AssetId assetId);
}