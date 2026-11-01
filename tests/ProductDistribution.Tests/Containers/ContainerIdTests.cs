using ProductDistribution.Containers;
using Xunit;

namespace ProductDistribution.Tests.Containers;

public class ContainerIdTests
{
    [Fact]
    public void ConstructorWithValidValueCreatesContainerId()
    {
        // Arrange

        // Act
        var id = new ContainerId(1);

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
            new ContainerId(value);
        };

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(act);
    }
}
