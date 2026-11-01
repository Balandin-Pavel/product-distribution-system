namespace ProductDistribution.Containers;

public readonly record struct ContainerId
{
    public ContainerId(int value)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);

        Value = value;
    }

    public int Value { get; }
}
