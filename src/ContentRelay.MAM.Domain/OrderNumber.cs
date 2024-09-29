using System.Text.RegularExpressions;
using ContentRelay.Shared;

namespace ContentRelay.MAM.Domain;

public partial record OrderNumber : ValueObject
{
    public static OrderNumber Empty => new("MTY000000000");
    
    public string Value { get; }

    private OrderNumber(string value)
    {
        Value = value;
    }

    public static OneOf<OrderNumber, ValidationError> From(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return OrderNumberError.InvalidOrderNumber;
        }
        
        var value = id.ToUpper();
        
        // OrderNumber must be 12 characters long (ORD123456789)
        if (value.Length != 12)
        {
            return AssetIdError.InvalidAssetId;
        }
        
        return OrderNumberRegex().IsMatch(value) 
            ? new OrderNumber(id)
            : OrderNumberError.InvalidOrderNumber;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    [GeneratedRegex("(ORD)\\d{9}")]
    private static partial Regex OrderNumberRegex();
}

public record OrderNumberError(string Message) : ValidationError(Message)
{
    public static OrderNumberError InvalidOrderNumber => new("Order number must be in the format 'ORD000000001'");
}