namespace ContentRelay.MAM.Domain;

public abstract record ValueObject
{
    // Derived Value Objects will implement this to define equality components
    protected abstract IEnumerable<object> GetEqualityComponents();

    // Override equality comparison for records based on the equality components
    public virtual bool Equals(ValueObject? other)
    {
        if (other == null || other.GetType() != GetType())
            return false;
        
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(1, (current, obj) => HashCode.Combine(current, obj?.GetHashCode() ?? 0));
    }
}