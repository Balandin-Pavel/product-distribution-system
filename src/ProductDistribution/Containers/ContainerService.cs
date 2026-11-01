using ProductDistribution.Containers.Results;
using ProductDistribution.ProductGroups;
using ProductDistribution.Products;

namespace ProductDistribution.Containers;

public sealed class ContainerService : IContainerService
{
    private readonly IContainerRepository _repository;
    private readonly IReadOnlyCollection<ProductGroup> _productGroups;

    public ContainerService(IContainerRepository repository) : this(repository, [])
    {
    }

    public ContainerService(IContainerRepository repository, IReadOnlyCollection<ProductGroup> productGroups)
    {
        var categories = productGroups
            .SelectMany(productGroup => productGroup.Categories)
            .ToList();

        if (categories.Count != categories.Distinct().Count())
        {
            throw new ArgumentException("A category cannot belong to multiple product groups", nameof(productGroups));
        }

        _repository = repository;
        _productGroups = productGroups.ToList();
    }

    public ContainerAddResult Add(Container container, ProductInstance productInstance)
    {
        IReadOnlyCollection<ProductInstance> contents = _repository.GetContents(container.Id);

        ProductInstance? conflictingProduct = contents.FirstOrDefault(storedProduct => BelongsToSameProductGroup(storedProduct, productInstance) is false);

        if (conflictingProduct is not null)
        {
            return new ContainerAddResult.ProductNeighborhoodViolation(conflictingProduct, productInstance);
        }

        Ovh storedOvh = contents
            .Select(storedProduct => storedProduct.Sku.Ovh)
            .Aggregate(new Ovh(0, 0), (totalOvh, productOvh) => totalOvh + productOvh);

        Ovh resultingOvh = storedOvh + productInstance.Sku.Ovh;

        if (resultingOvh <= container.MaximumOvh is false)
        {
            return new ContainerAddResult.InsufficientCapacity(container.MaximumOvh, storedOvh, productInstance.Sku.Ovh);
        }

        _repository.Add(container.Id, productInstance);

        return new ContainerAddResult.Success();
    }

    private bool BelongsToSameProductGroup(ProductInstance first, ProductInstance second)
    {
        ProductGroup? firstGroup = FindProductGroup(first.Sku.CategoryId);
        ProductGroup? secondGroup = FindProductGroup(second.Sku.CategoryId);

        if (firstGroup is null || secondGroup is null)
        {
            return firstGroup is null && secondGroup is null;
        }

        return firstGroup.Id == secondGroup.Id;
    }

    private ProductGroup? FindProductGroup(CategoryId categoryId)
    {
        return _productGroups.FirstOrDefault(productGroup => productGroup.Contains(categoryId));
    }
}
