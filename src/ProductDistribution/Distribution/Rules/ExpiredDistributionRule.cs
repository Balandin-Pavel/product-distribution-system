using ProductDistribution.Distribution.Results;
using ProductDistribution.Products;
using ProductDistribution.Time;

namespace ProductDistribution.Distribution.Rules;

public sealed class ExpiredDistributionRule : IDistributionRule
{
    private readonly IDistributionRule _innerRule;
    private readonly ICurrentTimeProvider _currentTimeProvider;

    public ExpiredDistributionRule(IDistributionRule innerRule, ICurrentTimeProvider currentTimeProvider)
    {
        _innerRule = innerRule;
        _currentTimeProvider = currentTimeProvider;
    }

    public DistributionResult Distribute(ProductInstance productInstance)
    {
        if (productInstance.ExpirationDate is null || productInstance.ExpirationDate >= _currentTimeProvider.Current)
        {
            return new DistributionResult.Failure();
        }

        return _innerRule.Distribute(productInstance);
    }
}
