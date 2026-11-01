using ProductDistribution.Distribution.Results;
using ProductDistribution.Products;

namespace ProductDistribution.Distribution.Rules;

public sealed class CategoryRouterRule : IDistributionRule
{
    private readonly IReadOnlyCollection<CategoryRuleMapping> _mappings;
    private readonly IDistributionRule _defaultRule;

    public CategoryRouterRule(IReadOnlyCollection<CategoryRuleMapping> mappings, IDistributionRule defaultRule)
    {
        if (mappings
            .Select(mapping => mapping.CategoryId)
            .Distinct()
            .Count() != mappings.Count)
        {
            throw new ArgumentException("Category mappings must not contain duplicate categories", nameof(mappings));
        }

        _mappings = mappings.ToList();
        _defaultRule = defaultRule;
    }

    public DistributionResult Distribute(ProductInstance productInstance)
    {
        CategoryRuleMapping? mapping = _mappings.FirstOrDefault(item => item.CategoryId == productInstance.Sku.CategoryId);

        IDistributionRule rule = mapping?.Rule ?? _defaultRule;

        return rule.Distribute(productInstance);
    }
}
