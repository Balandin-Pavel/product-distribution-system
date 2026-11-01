namespace ProductDistribution.ProductGroups;

public readonly record struct ProductGroupId
{
    public ProductGroupId(int value)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);

        Value = value;
    }

    public int Value { get; }
}
