using ProductDistribution.Containers;
using ProductDistribution.Distribution;
using ProductDistribution.Distribution.Results;
using ProductDistribution.Distribution.Rules;
using ProductDistribution.Products;
using ProductDistribution.Tests.TestDoubles;
using Xunit;

namespace ProductDistribution.Tests.Distribution.Rules;

public class PalletDistributionRuleTests
{
    [Fact]
    public void DistributeExpiredProductStoresItInExpiredProductsContainer()
    {
        // Arrange
        var currentTime = new DateTimeOffset(2026, 1, 2, 0, 0, 0, TimeSpan.Zero);
        var repository = new InMemoryContainerRepository();
        var service = new ContainerService(repository);
        var nonExpiredContainer = new Container(new ContainerId(1), new Ovh(10, 10));
        var expiredContainer = new Container(new ContainerId(2), new Ovh(10, 10));
        var nonExpiredRule = new NonExpiredDistributionRule(
            new ContainerDistributionRule(nonExpiredContainer, service),
            new FixedCurrentTimeProvider(currentTime));
        var expiredRule = new ExpiredDistributionRule(
            new ContainerDistributionRule(expiredContainer, service),
            new FixedCurrentTimeProvider(currentTime));
        var palletRule = new PalletDistributionRule([nonExpiredRule, expiredRule]);
        var product = new ProductInstance(
            new ProductInstanceId(1),
            new Sku(new SkuId(1), new Ovh(1, 1), new CategoryId(1)),
            currentTime.AddDays(-1));

        // Act
        DistributionResult result = palletRule.Distribute(product);

        // Assert
        Assert.IsType<DistributionResult.Success>(result);
        Assert.Empty(repository.GetContents(nonExpiredContainer.Id));
        Assert.Contains(product, repository.GetContents(expiredContainer.Id));
    }

    [Fact]
    public void DistributeStopsAfterFirstSuccessfulRule()
    {
        // Arrange
        var firstRule = new StubDistributionRule(new DistributionResult.Failure());
        var secondRule = new StubDistributionRule(new DistributionResult.Success());
        var skippedRule = new StubDistributionRule(new DistributionResult.Success());
        var rules = new IDistributionRule[] { firstRule, secondRule, skippedRule };
        var palletRule = new PalletDistributionRule(rules);
        var product = new ProductInstance(
            new ProductInstanceId(1),
            new Sku(new SkuId(1), new Ovh(1, 1), new CategoryId(1)));

        // Act
        DistributionResult result = palletRule.Distribute(product);

        // Assert
        Assert.IsType<DistributionResult.Success>(result);
        Assert.Equal(1, firstRule.CallCount);
        Assert.Equal(1, secondRule.CallCount);
        Assert.Equal(0, skippedRule.CallCount);
    }

    [Fact]
    public void DistributeWhenAllRulesFailReturnsFailure()
    {
        // Arrange
        var firstRule = new StubDistributionRule(new DistributionResult.Failure());
        var secondRule = new StubDistributionRule(new DistributionResult.Failure());
        var rules = new IDistributionRule[] { firstRule, secondRule };
        var palletRule = new PalletDistributionRule(rules);
        var product = new ProductInstance(
            new ProductInstanceId(1),
            new Sku(new SkuId(1), new Ovh(1, 1), new CategoryId(1)));

        // Act
        DistributionResult result = palletRule.Distribute(product);

        // Assert
        Assert.IsType<DistributionResult.Failure>(result);
        Assert.Equal(1, firstRule.CallCount);
        Assert.Equal(1, secondRule.CallCount);
    }
}
