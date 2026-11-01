using ProductDistribution.Products;
using Xunit;

namespace ProductDistribution.Tests.Products;

public class ProductInstanceIdTests
{
    [Fact]
    public void ConstructorWithValidValueCreatesProductInstanceId()
    {
        // Arrange

        // Act
        var id = new ProductInstanceId(1);

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
            new ProductInstanceId(value);
        };

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(act);
    }
}
