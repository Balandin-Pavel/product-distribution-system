using ProductDistribution.Products;

namespace ProductDistribution.Distribution.Rules;

public sealed record CategoryRuleMapping
{
    public CategoryRuleMapping(CategoryId categoryId, IDistributionRule rule)
    {
        CategoryId = categoryId;
        Rule = rule;
    }

    public CategoryId CategoryId { get; }

    public IDistributionRule Rule { get; }
}
