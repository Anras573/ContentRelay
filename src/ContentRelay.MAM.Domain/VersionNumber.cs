using ContentRelay.Shared;

namespace ContentRelay.MAM.Domain;

public record VersionNumber : ValueObject
{
    public static VersionNumber Empty => new(0, 0);
    public int Major { get; }
    public int Minor { get; }
    public string Value => $"{Major}.{Minor}";
    
    private VersionNumber(int major, int minor)
    {
        Major = major;
        Minor = minor;
    }

    public static OneOf<VersionNumber, ValidationError> From(string version)
    {
        var parts = version.Split('.');
        if (parts.Length != 2)
        {
            return VersionNumberError.InvalidVersion;
        }
        
        if (!int.TryParse(parts[0], out var major))
        {
            return VersionNumberError.InvalidMajor;
        }
        
        if (!int.TryParse(parts[1], out var minor))
        {
            return VersionNumberError.InvalidMinor;
        }
        
        return new VersionNumber(major, minor);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Major;
        yield return Minor;
    }
}

public record VersionNumberError(string Message) : ValidationError(Message)
{
    public static VersionNumberError InvalidVersion => new($"Version number must be in the format 'major.minor'");
    public static VersionNumberError InvalidMajor => new("Major version number must be an integer");
    public static VersionNumberError InvalidMinor => new("Minor version number must be an integer");
}