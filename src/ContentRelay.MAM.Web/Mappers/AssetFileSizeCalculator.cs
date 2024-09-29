using System.Security.Cryptography;
using ContentRelay.MAM.Application.Mappers;
using ContentRelay.MAM.Web.Errors;
using ContentRelay.Shared;

namespace ContentRelay.MAM.Web.Mappers;

public class AssetFileSizeCalculator : IAssetFileSizeCalculator
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AssetFileSizeCalculator(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public OneOf<long, CouldNotCalculateFileSizeError> CalculateSize(Uri path)
    {
        var client = _httpClientFactory.CreateClient();
        
        // Simulate a HEAD request to get the file size
        // if (TryGetFileSizeByHEadRequest(client, path, out var fileSize))
        // {
        //     return fileSize;
        // }
        
        // Simulate a GET request to get the file size
        // if (TryGetFileSizeByDownloadingAsset(client, path, out var fileSize))
        // {
        //     return fileSize;
        // }

        var size = RandomNumberGenerator.GetInt32(int.MaxValue);
        
        return size > 0
            ? size
            : CouldNotCalculateFileSizeError.CouldNotCalculateFileSize(path);
    }
}