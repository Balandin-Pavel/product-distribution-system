using ProductDistribution.Distribution.Results;
using ProductDistribution.Distribution.Rules;
using ProductDistribution.Products;
using ProductDistribution.Tests.TestDoubles;
using Xunit;

namespace ProductDistribution.Tests.Distribution.Rules;

public class ExpiredDistributionRuleTests
{
    [Fact]
    public void DistributeExpiredProductCallsInnerRule()
    {
        // Arrange
        var currentTime = new DateTimeOffset(2026, 1, 2, 0, 0, 0, TimeSpan.Zero);
        var innerRule = new StubDistributionRule(new DistributionResult.Success());
        var rule = new ExpiredDistributionRule(innerRule, new FixedCurrentTimeProvider(currentTime));
        ProductInstance product = CreateProduct(currentTime.AddDays(-1));

        // Act
        DistributionResult result = rule.Distribute(product);

        // Assert
        Assert.IsType<DistributionResult.Success>(result);
        Assert.Equal(1, innerRule.CallCount);
    }

    [Fact]
    public void DistributeProductWithoutExpirationDateReturnsFailureWithoutCallingInnerRule()
    {
        // Arrange
        var currentTime = new DateTimeOffset(2026, 1, 2, 0, 0, 0, TimeSpan.Zero);
        var innerRule = new StubDistributionRule(new DistributionResult.Success());
        var rule = new ExpiredDistributionRule(innerRule, new FixedCurrentTimeProvider(currentTime));
        ProductInstance product = CreateProduct(null);

        // Act
        DistributionResult result = rule.Distribute(product);

        // Assert
        Assert.IsType<DistributionResult.Failure>(result);
        Assert.Equal(0, innerRule.CallCount);
    }

    [Fact]
    public void DistributeNonExpiredProductReturnsFailureWithoutCallingInnerRule()
    {
        // Arrange
        var currentTime = new DateTimeOffset(2026, 1, 2, 0, 0, 0, TimeSpan.Zero);
        var innerRule = new StubDistributionRule(new DistributionResult.Success());
        var rule = new ExpiredDistributionRule(innerRule, new FixedCurrentTimeProvider(currentTime));
        ProductInstance product = CreateProduct(currentTime.AddDays(1));

        // Act
        DistributionResult result = rule.Distribute(product);

        // Assert
        Assert.IsType<DistributionResult.Failure>(result);
        Assert.Equal(0, innerRule.CallCount);
    }

    private static ProductInstance CreateProduct(DateTimeOffset? expirationDate)
    {
        return new ProductInstance(
            new ProductInstanceId(1),
            new Sku(new SkuId(1), new Ovh(1, 1), new CategoryId(1)),
            expirationDate);
    }
}
