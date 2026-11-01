namespace ProductDistribution.Products;

public readonly record struct ProductInstanceId
{
    public ProductInstanceId(int value)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);

        Value = value;
    }

    public int Value { get; }
}
