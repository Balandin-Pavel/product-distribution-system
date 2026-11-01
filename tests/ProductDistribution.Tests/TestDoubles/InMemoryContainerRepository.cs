using ProductDistribution.Containers;
using ProductDistribution.Products;

namespace ProductDistribution.Tests.TestDoubles;

internal sealed class InMemoryContainerRepository : IContainerRepository
{
    private readonly Dictionary<ContainerId, List<ProductInstance>> _contents = [];

    public IReadOnlyCollection<ProductInstance> GetContents(ContainerId containerId)
    {
        return _contents.TryGetValue(containerId, out List<ProductInstance>? contents)
            ? contents.ToList()
            : [];
    }

    public void Add(ContainerId containerId, ProductInstance productInstance)
    {
        if (_contents.TryGetValue(containerId, out List<ProductInstance>? contents))
        {
            contents.Add(productInstance);
            return;
        }

        _contents[containerId] = [productInstance];
    }
}
