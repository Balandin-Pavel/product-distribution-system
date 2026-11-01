using ProductDistribution.Containers;
using ProductDistribution.Distribution.Results;
using ProductDistribution.Distribution.Rules;
using ProductDistribution.Products;
using ProductDistribution.Tests.TestDoubles;
using Xunit;

namespace ProductDistribution.Tests.Distribution.Rules;

public class LoggingDistributionRuleTests
{
    [Fact]
    public void DistributeSuccessfulAndFailedProductsLogsBothResults()
    {
        // Arrange
        var logger = new SpyDistributionLogger();
        var repository = new InMemoryContainerRepository();
        var container = new Container(new ContainerId(1), new Ovh(1, 1));
        var rule = new LoggingDistributionRule(
            new ContainerDistributionRule(container, new ContainerService(repository)),
            logger);
        var firstProduct = new ProductInstance(
            new ProductInstanceId(1),
            new Sku(new SkuId(1), new Ovh(1, 1), new CategoryId(1)));
        var secondProduct = new ProductInstance(
            new ProductInstanceId(2),
            new Sku(new SkuId(2), new Ovh(1, 1), new CategoryId(1)));

        // Act
        DistributionResult successfulResult = rule.Distribute(firstProduct);
        DistributionResult failedResult = rule.Distribute(secondProduct);

        // Assert
        Assert.IsType<DistributionResult.Success>(successfulResult);
        Assert.IsType<DistributionResult.Failure>(failedResult);
        Assert.Collection(
            logger.Messages,
            message => Assert.Equal("Product distribution succeeded", message),
            message => Assert.Equal("Product distribution failed", message));
    }
}
