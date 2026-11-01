using ProductDistribution.Distribution;
using ProductDistribution.Distribution.Results;
using ProductDistribution.Products;

namespace ProductDistribution.Tests.TestDoubles;

internal sealed class StubDistributionRule : IDistributionRule
{
    private readonly DistributionResult _result;

    public StubDistributionRule(DistributionResult result)
    {
        _result = result;
    }

    public int CallCount { get; private set; }

    public DistributionResult Distribute(ProductInstance productInstance)
    {
        CallCount++;
        return _result;
    }
}
