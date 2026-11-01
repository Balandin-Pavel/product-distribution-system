using ProductDistribution.Distribution.Results;
using ProductDistribution.Products;

namespace ProductDistribution.Distribution.Rules;

public sealed class PalletDistributionRule : IDistributionRule
{
    private readonly IReadOnlyCollection<IDistributionRule> _rules;

    public PalletDistributionRule(IReadOnlyCollection<IDistributionRule> rules)
    {
        if (rules.Count == 0)
        {
            throw new ArgumentException("Pallet must contain at least one rule", nameof(rules));
        }

        _rules = rules.ToList();
    }

    public DistributionResult Distribute(ProductInstance productInstance)
    {
        DistributionResult? success = _rules
            .Select(rule => rule.Distribute(productInstance))
            .FirstOrDefault(result => result is DistributionResult.Success);

        return success ?? new DistributionResult.Failure();
    }
}
