using System.Text.RegularExpressions;
using ContentRelay.Shared;

namespace ContentRelay.MAM.Domain;

public partial record BriefId : ValueObject
{
    public BriefId(string value) => Value = value;

    public string Value { get; }
    
    public static OneOf<BriefId, ValidationError> From(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BriefIdError.InvalidBriefId;
        }

        var value = id.ToUpper();
        
        // Brief Id must be 8 characters long (BRIEF001)
        if (value.Length != 8)
        {
            return BriefIdError.InvalidBriefId;
        }
        
        return BriefIdRegex().IsMatch(value) 
            ? new BriefId(id)
            : BriefIdError.InvalidBriefId;
    }

    [GeneratedRegex("(BRIEF)\\d{3}")]
    private static partial Regex BriefIdRegex();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public record BriefIdError(string Message) : ValidationError(Message)
{
    public static BriefIdError InvalidBriefId => new("Brief id must be in the format 'BRIEF001'");
}