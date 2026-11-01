using System.Diagnostics;
using ProductDistribution.Distribution.Results;
using ProductDistribution.Logging;
using ProductDistribution.Products;

namespace ProductDistribution.Distribution.Rules;

public sealed class LoggingDistributionRule : IDistributionRule
{
    private readonly IDistributionRule _innerRule;
    private readonly IDistributionLogger _logger;

    public LoggingDistributionRule(IDistributionRule innerRule, IDistributionLogger logger)
    {
        _innerRule = innerRule;
        _logger = logger;
    }

    public DistributionResult Distribute(ProductInstance productInstance)
    {
        DistributionResult result = _innerRule.Distribute(productInstance);

        string message = result switch
        {
            DistributionResult.Success => "Product distribution succeeded",
            DistributionResult.Failure => "Product distribution failed",
            _ => throw new UnreachableException("Distribution rule returned an unknown result"),
        };

        _logger.Log(message);

        return result;
    }
}
