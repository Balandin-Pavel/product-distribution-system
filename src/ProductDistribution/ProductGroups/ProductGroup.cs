using ProductDistribution.Products;

namespace ProductDistribution.ProductGroups;

public sealed class ProductGroup
{
    private readonly IReadOnlyCollection<CategoryId> _categories;

    public ProductGroup(ProductGroupId id, IReadOnlyCollection<CategoryId> categories)
    {
        if (categories.Count == 0)
        {
            throw new ArgumentException("Product group must contain at least one category", nameof(categories));
        }

        Id = id;
        _categories = categories.Distinct().ToList();
    }

    public ProductGroupId Id { get; }

    public IReadOnlyCollection<CategoryId> Categories => _categories.ToList();

    public bool Contains(CategoryId categoryId)
    {
        return _categories.Contains(categoryId);
    }
}
