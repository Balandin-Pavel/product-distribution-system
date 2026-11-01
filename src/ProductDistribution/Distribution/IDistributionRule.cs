using ProductDistribution.Distribution.Results;
using ProductDistribution.Products;

namespace ProductDistribution.Distribution;

public interface IDistributionRule
{
    DistributionResult Distribute(ProductInstance productInstance);
}
