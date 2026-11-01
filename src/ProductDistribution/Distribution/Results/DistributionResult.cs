namespace ProductDistribution.Distribution.Results;

public abstract record DistributionResult
{
    public sealed record Success : DistributionResult;

    public sealed record Failure : DistributionResult;
}
