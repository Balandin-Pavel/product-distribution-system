using ProductDistribution.Products;
using Xunit;

namespace ProductDistribution.Tests.Products;

public class CategoryIdTests
{
    [Fact]
    public void ConstructorWithValidValueCreatesCategoryId()
    {
        // Arrange

        // Act
        var id = new CategoryId(1);

        // Assert
        Assert.Equal(1, id.Value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ConstructorWithInvalidValueThrows(int value)
    {
        // Arrange

        // Act
        Action act = () =>
        {
            new CategoryId(value);
        };

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(act);
    }
}
