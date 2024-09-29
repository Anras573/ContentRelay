using ContentRelay.Shared;

namespace ContentRelay.MAM.Domain;

public record FileFormat : ValueObject
{
    public static FileFormat Empty => new("ToBeDefined");
    private static readonly HashSet<string> AllowedFormats = ["jpg", "png", "pdf", "mp4", "mov", "mp3", "wav", "obj"];
    
    public string Value { get; }
    
    private FileFormat(string value)
    {
        Value = value;
    }
    
    public static OneOf<FileFormat, ValidationError> From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return FileFormatError.Empty;
        }
        
        if (!AllowedFormats.Contains(value))
        {
            return FileFormatError.Invalid(value);
        }
        
        return new FileFormat(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public record FileFormatError(string Message) : ValidationError(Message)
{
    public static FileFormatError Empty => new("File format must not be empty");
    public static FileFormatError Invalid(string format) => new($"File format {format} is not allowed");
}