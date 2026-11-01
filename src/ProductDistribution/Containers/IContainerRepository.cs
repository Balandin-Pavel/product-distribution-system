using ProductDistribution.Products;

namespace ProductDistribution.Containers;

public interface IContainerRepository
{
    IReadOnlyCollection<ProductInstance> GetContents(ContainerId containerId);

    void Add(ContainerId containerId, ProductInstance productInstance);
}
