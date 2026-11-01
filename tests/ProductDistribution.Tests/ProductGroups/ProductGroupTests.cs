using ProductDistribution.ProductGroups;
using ProductDistribution.Products;
using Xunit;

namespace ProductDistribution.Tests.ProductGroups;

public class ProductGroupTests
{
    [Fact]
    public void ConstructorWithEmptyCategoriesThrows()
    {
        // Arrange
        var categories = new List<CategoryId>();

        // Act
        Action act = () =>
        {
            new ProductGroup(new ProductGroupId(1), categories);
        };

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void ConstructorRemovesDuplicateCategories()
    {
        // Arrange
        var category = new CategoryId(1);

        // Act
        var productGroup = new ProductGroup(new ProductGroupId(1), [category, category]);

        // Assert
        Assert.Single(productGroup.Categories);
        Assert.Contains(category, productGroup.Categories);
    }

    [Fact]
    public void ConstructorCopiesCategoryCollection()
    {
        // Arrange
        var category = new CategoryId(1);
        var categories = new List<CategoryId> { category };
        var productGroup = new ProductGroup(new ProductGroupId(1), categories);

        // Act
        categories.Add(new CategoryId(2));

        // Assert
        Assert.Single(productGroup.Categories);
        Assert.Contains(category, productGroup.Categories);
    }

    [Fact]
    public void CategoriesReturnsDefensiveCopy()
    {
        // Arrange
        var category = new CategoryId(1);
        var productGroup = new ProductGroup(new ProductGroupId(1), [category]);
        ICollection<CategoryId> returnedCategories = Assert.IsAssignableFrom<ICollection<CategoryId>>(
            productGroup.Categories);

        // Act
        returnedCategories.Add(new CategoryId(2));

        // Assert
        Assert.Single(productGroup.Categories);
        Assert.Contains(category, productGroup.Categories);
    }

    [Fact]
    public void ContainsReturnsWhetherCategoryBelongsToGroup()
    {
        // Arrange
        var includedCategory = new CategoryId(1);
        var excludedCategory = new CategoryId(2);
        var productGroup = new ProductGroup(new ProductGroupId(1), [includedCategory]);

        // Act
        bool containsIncluded = productGroup.Contains(includedCategory);
        bool containsExcluded = productGroup.Contains(excludedCategory);

        // Assert
        Assert.True(containsIncluded);
        Assert.False(containsExcluded);
    }
}
