using ProductDistribution.ProductGroups;
using Xunit;

namespace ProductDistribution.Tests.ProductGroups;

public class ProductGroupIdTests
{
    [Fact]
    public void ConstructorWithValidValueCreatesProductGroupId()
    {
        // Arrange

        // Act
        var id = new ProductGroupId(1);

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
            new ProductGroupId(value);
        };

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(act);
    }
}
