using ProductDistribution.Time;

namespace ProductDistribution.Tests.TestDoubles;

internal sealed class FixedCurrentTimeProvider : ICurrentTimeProvider
{
    public FixedCurrentTimeProvider(DateTimeOffset current)
    {
        Current = current;
    }

    public DateTimeOffset Current { get; }
}
