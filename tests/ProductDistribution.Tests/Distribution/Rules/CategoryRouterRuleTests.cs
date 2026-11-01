using ProductDistribution.Containers;
using ProductDistribution.Distribution.Results;
using ProductDistribution.Distribution.Rules;
using ProductDistribution.Products;
using ProductDistribution.Tests.TestDoubles;
using Xunit;

namespace ProductDistribution.Tests.Distribution.Rules;

public class CategoryRouterRuleTests
{
    [Fact]
    public void DistributeWithNoMappingUsesDefaultRule()
    {
        // Arrange
        var repository = new InMemoryContainerRepository();
        var container = new Container(new ContainerId(1), new Ovh(10, 10));
        var defaultRule = new ContainerDistributionRule(container, new ContainerService(repository));
        var router = new CategoryRouterRule([], defaultRule);
        var product = new ProductInstance(
            new ProductInstanceId(1),
            new Sku(new SkuId(1), new Ovh(1, 1), new CategoryId(1)));

        // Act
        DistributionResult result = router.Distribute(product);

        // Assert
        Assert.IsType<DistributionResult.Success>(result);
        Assert.Contains(product, repository.GetContents(container.Id));
    }

    [Fact]
    public void DistributeMappedCategoriesUsesCorrespondingRules()
    {
        // Arrange
        var electronicsCategory = new CategoryId(1);
        var freshFoodCategory = new CategoryId(2);
        var electronicsProduct = new ProductInstance(
            new ProductInstanceId(1),
            new Sku(new SkuId(1), new Ovh(1, 1), electronicsCategory));
        var freshFoodProduct = new ProductInstance(
            new ProductInstanceId(2),
            new Sku(new SkuId(2), new Ovh(1, 1), freshFoodCategory));
        var electronicsContainer = new Container(new ContainerId(1), new Ovh(10, 10));
        var freshFoodContainer = new Container(new ContainerId(2), new Ovh(10, 10));
        var defaultContainer = new Container(new ContainerId(3), new Ovh(10, 10));
        var repository = new InMemoryContainerRepository();
        var service = new ContainerService(repository);
        var mappings = new CategoryRuleMapping[]
        {
            new(electronicsCategory, new ContainerDistributionRule(electronicsContainer, service)),
            new(freshFoodCategory, new ContainerDistributionRule(freshFoodContainer, service)),
        };
        var router = new CategoryRouterRule(
            mappings,
            new ContainerDistributionRule(defaultContainer, service));

        // Act
        DistributionResult electronicsResult = router.Distribute(electronicsProduct);
        DistributionResult freshFoodResult = router.Distribute(freshFoodProduct);

        // Assert
        Assert.IsType<DistributionResult.Success>(electronicsResult);
        Assert.IsType<DistributionResult.Success>(freshFoodResult);
        Assert.Contains(electronicsProduct, repository.GetContents(electronicsContainer.Id));
        Assert.Contains(freshFoodProduct, repository.GetContents(freshFoodContainer.Id));
        Assert.Empty(repository.GetContents(defaultContainer.Id));
    }

    [Fact]
    public void ConstructorWithDuplicateCategoryMappingsThrows()
    {
        // Arrange
        var category = new CategoryId(1);
        var rule = new StubDistributionRule(new DistributionResult.Success());
        var mappings = new CategoryRuleMapping[]
        {
            new(category, rule),
            new(category, rule),
        };

        // Act
        Action act = () =>
        {
            new CategoryRouterRule(mappings, rule);
        };

        // Assert
        Assert.Throws<ArgumentException>(act);
    }
}
