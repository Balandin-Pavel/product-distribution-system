namespace ProductDistribution.Products;

public readonly record struct CategoryId
{
    public CategoryId(int value)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);

        Value = value;
    }

    public int Value { get; }
}
