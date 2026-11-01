using ProductDistribution.Containers;
using ProductDistribution.Products;
using Xunit;

namespace ProductDistribution.Tests.Containers;

public class ContainerTests
{
    [Fact]
    public void ConstructorWithValidArgumentsCreatesContainer()
    {
        // Arrange
        var id = new ContainerId(1);
        var maximumOvh = new Ovh(10, 5);

        // Act
        var container = new Container(id, maximumOvh);

        // Assert
        Assert.Equal(id, container.Id);
        Assert.Equal(maximumOvh, container.MaximumOvh);
    }

    [Fact]
    public void ConstructorWithZeroMaximumOvhThrows()
    {
        // Arrange

        // Act
        Action act = () =>
        {
            new Container(new ContainerId(1), new Ovh(0, 0));
        };

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(act);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    public void ConstructorWithOneZeroMaximumOvhComponentCreatesContainer(double volume, double weight)
    {
        // Arrange
        var maximumOvh = new Ovh(volume, weight);

        // Act
        var container = new Container(new ContainerId(1), maximumOvh);

        // Assert
        Assert.Equal(maximumOvh, container.MaximumOvh);
    }
}
