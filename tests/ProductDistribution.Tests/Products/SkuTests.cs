using ProductDistribution.Products;
using Xunit;

namespace ProductDistribution.Tests.Products;

public class SkuTests
{
    [Fact]
    public void ConstructorWithValidArgumentsCreatesSku()
    {
        // Arrange
        var id = new SkuId(1);
        var categoryId = new CategoryId(1);
        var ovh = new Ovh(10, 5);

        // Act
        var sku = new Sku(id, ovh, categoryId);

        // Assert
        Assert.Equal(id, sku.Id);
        Assert.Equal(categoryId, sku.CategoryId);
        Assert.Equal(ovh, sku.Ovh);
    }

    [Fact]
    public void ConstructorWithZeroOvhThrows()
    {
        // Arrange

        // Act
        Action act = () =>
        {
            new Sku(new SkuId(1), new Ovh(0, 0), new CategoryId(1));
        };

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(act);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    public void ConstructorWithOneZeroOvhComponentCreatesSku(double volume, double weight)
    {
        // Arrange
        var ovh = new Ovh(volume, weight);

        // Act
        var sku = new Sku(new SkuId(1), ovh, new CategoryId(1));

        // Assert
        Assert.Equal(ovh, sku.Ovh);
    }
}
