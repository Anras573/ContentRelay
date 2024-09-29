namespace ContentRelay.MAM.Web.Errors;

public record CouldNotCalculateFileSizeError(string Message)
{
    public static CouldNotCalculateFileSizeError CouldNotCalculateFileSize(Uri path) => new($"Could not calculate file size for path: {path}");
}