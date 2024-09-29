using ContentRelay.Shared;

namespace ContentRelay.MAM.Domain;

public record UserName : ValueObject
{
    public static UserName Empty => new("ToBeDefined");
    public string Value { get; }
    
    private UserName(string value)
    {
        Value = value;
    }
    
    public static OneOf<UserName, ValidationError> From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return UserNameError.InvalidName;
        }
        
        return new UserName(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public record UserNameError(string Message) : ValidationError(Message)
{
    public static UserNameError InvalidName => new("User name must not be empty");
}