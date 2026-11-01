using ProductDistribution.Products;
using Xunit;

namespace ProductDistribution.Tests.Products;

public class ProductInstanceTests
{
    [Fact]
    public void ConstructorWithoutExpirationDateCreatesProductInstance()
    {
        // Arrange
        var id = new ProductInstanceId(1);
        var sku = new Sku(new SkuId(1), new Ovh(10, 5), new CategoryId(1));

        // Act
        var productInstance = new ProductInstance(id, sku);

        // Assert
        Assert.Equal(id, productInstance.Id);
        Assert.Same(sku, productInstance.Sku);
        Assert.Null(productInstance.ExpirationDate);
    }

    [Fact]
    public void ConstructorWithExpirationDateCreatesProductInstance()
    {
        // Arrange
        var expirationDate = new DateTimeOffset(2026, 10, 11, 12, 0, 0, TimeSpan.Zero);
        var sku = new Sku(new SkuId(1), new Ovh(10, 5), new CategoryId(1));

        // Act
        var productInstance = new ProductInstance(new ProductInstanceId(1), sku, expirationDate);

        // Assert
        Assert.Equal(expirationDate, productInstance.ExpirationDate);
    }
}
