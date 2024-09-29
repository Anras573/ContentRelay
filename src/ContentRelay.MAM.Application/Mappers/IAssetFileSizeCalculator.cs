using ContentRelay.MAM.Web.Errors;
using ContentRelay.Shared;

namespace ContentRelay.MAM.Application.Mappers;

public interface IAssetFileSizeCalculator
{
    OneOf<long, CouldNotCalculateFileSizeError> CalculateSize(Uri path);
}