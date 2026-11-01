using ProductDistribution.Containers;
using ProductDistribution.Distribution.Results;
using ProductDistribution.Distribution.Rules;
using ProductDistribution.Products;
using ProductDistribution.Tests.TestDoubles;
using Xunit;

namespace ProductDistribution.Tests.Distribution.Rules;

public class ContainerDistributionRuleTests
{
    [Fact]
    public void DistributeCompatibleProductReturnsSuccessAndStoresProduct()
    {
        // Arrange
        var product = new ProductInstance(
            new ProductInstanceId(1),
            new Sku(new SkuId(1), new Ovh(1, 1), new CategoryId(1)));
        var container = new Container(new ContainerId(1), new Ovh(10, 10));
        var repository = new InMemoryContainerRepository();
        var rule = new ContainerDistributionRule(container, new ContainerService(repository));

        // Act
        DistributionResult result = rule.Distribute(product);

        // Assert
        Assert.IsType<DistributionResult.Success>(result);
        Assert.Contains(product, repository.GetContents(container.Id));
    }

    [Fact]
    public void DistributeProductExceedingCapacityReturnsFailureAndDoesNotStoreProduct()
    {
        // Arrange
        var product = new ProductInstance(
            new ProductInstanceId(1),
            new Sku(new SkuId(1), new Ovh(2, 2), new CategoryId(1)));
        var container = new Container(new ContainerId(1), new Ovh(1, 1));
        var repository = new InMemoryContainerRepository();
        var rule = new ContainerDistributionRule(container, new ContainerService(repository));

        // Act
        DistributionResult result = rule.Distribute(product);

        // Assert
        Assert.IsType<DistributionResult.Failure>(result);
        Assert.Empty(repository.GetContents(container.Id));
    }
}
