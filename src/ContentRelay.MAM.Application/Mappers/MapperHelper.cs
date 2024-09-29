using ContentRelay.MAM.Domain;
using ContentRelay.Shared;

namespace ContentRelay.MAM.Application.Mappers;

public static class MapperHelper
{
    public static T ValidateField<T>(
        string value, 
        Func<string, OneOf<T, ValidationError>> conversionFunc, 
        string fieldName, 
        ValidationErrors validationErrors) where T : class
    {
        var result = conversionFunc(value);
        return result.Match(
            success => success,
            error => {
                validationErrors.Add(fieldName, error.Message);
                return default;
            });
    }
    
    public static T ValidateField<T>(
        DateTimeOffset value, 
        Func<DateTimeOffset, OneOf<T, ValidationError>> conversionFunc, 
        string fieldName, 
        ValidationErrors validationErrors) where T : class
    {
        var result = conversionFunc(value);
        return result.Match(
            success => success,
            error => {
                validationErrors.Add(fieldName, error.Message);
                return default;
            });
    }
    
    public static T ValidateField<T>(
        DateTime value, 
        Func<DateTime, OneOf<T, ValidationError>> conversionFunc, 
        string fieldName, 
        ValidationErrors validationErrors) where T : class
    {
        var result = conversionFunc(value);
        return result.Match(
            success => success,
            error => {
                validationErrors.Add(fieldName, error.Message);
                return default;
            });
    }
    
    public static long ValidateFileSize(string path, IAssetFileSizeCalculator fileSizeCalculator, ValidationErrors validationErrors)
    {
        var uri = new Uri(path);
        var fileSizeOrError = fileSizeCalculator.CalculateSize(uri);
        return fileSizeOrError.Match(
            success => success,
            error => {
                validationErrors.Add("FileSize", error.Message);
                return 0;
            });
    }

    public static AssetStatus ValidateAssetStatus(string statusString, ValidationErrors validationErrors)
    {
        if (Enum.TryParse(typeof(AssetStatus), statusString, true, out var status) && status is AssetStatus validStatus)
        {
            return validStatus;
        }

        validationErrors.Add(nameof(AssetStatus), $"Invalid status: {statusString}");
        return default;
    }
    
    public static BriefStatus ValidateBriefStatus(string statusString, ValidationErrors validationErrors)
    {
        if (Enum.TryParse(typeof(BriefStatus), statusString, true, out var status) && status is BriefStatus validStatus)
        {
            return validStatus;
        }

        validationErrors.Add(nameof(BriefStatus), $"Invalid status: {statusString}");
        return default;
    }
    
    public static DistributionChannel ValidateDistributionChannel(string channelString, ValidationErrors validationErrors)
    {
        var normalizedChannelString = channelString.Replace(" ", "");
        
        if (Enum.TryParse(typeof(DistributionChannel), normalizedChannelString, true, out var channel) && channel is DistributionChannel validChannel)
        {
            return validChannel;
        }

        validationErrors.Add(nameof(DistributionChannel), $"Invalid distribution channel: {channelString}");
        return default;
    }
    
    public static DistributionMethod ValidateDistributionMethod(string methodString, ValidationErrors validationErrors)
    {
        if (Enum.TryParse(typeof(DistributionMethod), methodString, true, out var method) && method is DistributionMethod validMethod)
        {
            return validMethod;
        }

        validationErrors.Add(nameof(DistributionMethod), $"Invalid distribution method: {methodString}");
        return default;
    }
}