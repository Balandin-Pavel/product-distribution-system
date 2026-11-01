using ProductDistribution.Containers;
using ProductDistribution.Containers.Results;
using ProductDistribution.ProductGroups;
using ProductDistribution.Products;
using ProductDistribution.Tests.TestDoubles;
using Xunit;

namespace ProductDistribution.Tests.Containers;

public class ContainerServiceTests
{
    [Fact]
    public void ConstructorWithCategoryInMultipleProductGroupsThrows()
    {
        // Arrange
        var category = new CategoryId(1);
        var firstGroup = new ProductGroup(new ProductGroupId(1), [category]);
        var secondGroup = new ProductGroup(new ProductGroupId(2), [category]);

        // Act
        Action act = () =>
        {
            new ContainerService(new InMemoryContainerRepository(), [firstGroup, secondGroup]);
        };

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void AddProductFromDifferentProductGroupReturnsNeighborhoodViolation()
    {
        // Arrange
        var paperCategory = new CategoryId(1);
        var scissorsCategory = new CategoryId(2);
        var paperGroup = new ProductGroup(new ProductGroupId(1), [paperCategory]);
        var scissorsGroup = new ProductGroup(new ProductGroupId(2), [scissorsCategory]);
        var storedProduct = new ProductInstance(
            new ProductInstanceId(1),
            new Sku(new SkuId(1), new Ovh(1, 1), paperCategory));
        var incomingProduct = new ProductInstance(
            new ProductInstanceId(2),
            new Sku(new SkuId(2), new Ovh(1, 1), scissorsCategory));
        var container = new Container(new ContainerId(1), new Ovh(10, 10));
        var repository = new InMemoryContainerRepository();
        repository.Add(container.Id, storedProduct);
        var service = new ContainerService(repository, [paperGroup, scissorsGroup]);

        // Act
        ContainerAddResult result = service.Add(container, incomingProduct);

        // Assert
        Assert.IsType<ContainerAddResult.ProductNeighborhoodViolation>(result);
        Assert.Single(repository.GetContents(container.Id));
    }

    [Fact]
    public void AddProductFromSameProductGroupReturnsSuccess()
    {
        // Arrange
        var paperCategory = new CategoryId(1);
        var scissorsCategory = new CategoryId(2);
        var productGroup = new ProductGroup(new ProductGroupId(1), [paperCategory, scissorsCategory]);
        var storedProduct = new ProductInstance(
            new ProductInstanceId(1),
            new Sku(new SkuId(1), new Ovh(1, 1), paperCategory));
        var incomingProduct = new ProductInstance(
            new ProductInstanceId(2),
            new Sku(new SkuId(2), new Ovh(1, 1), scissorsCategory));
        var container = new Container(new ContainerId(1), new Ovh(10, 10));
        var repository = new InMemoryContainerRepository();
        repository.Add(container.Id, storedProduct);
        var service = new ContainerService(repository, [productGroup]);

        // Act
        ContainerAddResult result = service.Add(container, incomingProduct);

        // Assert
        Assert.IsType<ContainerAddResult.Success>(result);
        Assert.Contains(incomingProduct, repository.GetContents(container.Id));
    }

    [Fact]
    public void AddProductExceedingRemainingOvhReturnsInsufficientCapacity()
    {
        // Arrange
        var category = new CategoryId(1);
        var storedProduct = new ProductInstance(
            new ProductInstanceId(1),
            new Sku(new SkuId(1), new Ovh(5, 5), category));
        var incomingProduct = new ProductInstance(
            new ProductInstanceId(2),
            new Sku(new SkuId(2), new Ovh(1, 1), category));
        var container = new Container(new ContainerId(1), storedProduct.Sku.Ovh);
        var repository = new InMemoryContainerRepository();
        repository.Add(container.Id, storedProduct);
        var service = new ContainerService(repository);

        // Act
        ContainerAddResult result = service.Add(container, incomingProduct);

        // Assert
        Assert.IsType<ContainerAddResult.InsufficientCapacity>(result);
        Assert.Single(repository.GetContents(container.Id));
    }

    [Fact]
    public void AddUngroupedProductToContainerWithGroupedProductReturnsNeighborhoodViolation()
    {
        // Arrange
        var groupedCategory = new CategoryId(1);
        var ungroupedCategory = new CategoryId(2);
        var productGroup = new ProductGroup(new ProductGroupId(1), [groupedCategory]);
        var storedProduct = new ProductInstance(
            new ProductInstanceId(1),
            new Sku(new SkuId(1), new Ovh(1, 1), groupedCategory));
        var incomingProduct = new ProductInstance(
            new ProductInstanceId(2),
            new Sku(new SkuId(2), new Ovh(1, 1), ungroupedCategory));
        var container = new Container(new ContainerId(1), new Ovh(10, 10));
        var repository = new InMemoryContainerRepository();
        repository.Add(container.Id, storedProduct);
        var service = new ContainerService(repository, [productGroup]);

        // Act
        ContainerAddResult result = service.Add(container, incomingProduct);

        // Assert
        Assert.IsType<ContainerAddResult.ProductNeighborhoodViolation>(result);
        Assert.Single(repository.GetContents(container.Id));
    }

    [Fact]
    public void AddCompatibleProductReturnsSuccessAndStoresProduct()
    {
        // Arrange
        var category = new CategoryId(1);
        var product = new ProductInstance(
            new ProductInstanceId(1),
            new Sku(new SkuId(1), new Ovh(1, 1), category));
        var container = new Container(new ContainerId(1), new Ovh(10, 10));
        var repository = new InMemoryContainerRepository();
        var service = new ContainerService(repository);

        // Act
        ContainerAddResult result = service.Add(container, product);

        // Assert
        Assert.IsType<ContainerAddResult.Success>(result);
        Assert.Contains(product, repository.GetContents(container.Id));
    }
}
