namespace ProductDistribution.Time;

public interface ICurrentTimeProvider
{
    DateTimeOffset Current { get; }
}
