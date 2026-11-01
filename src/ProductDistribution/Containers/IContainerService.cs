using ProductDistribution.Containers.Results;
using ProductDistribution.Products;

namespace ProductDistribution.Containers;

public interface IContainerService
{
    ContainerAddResult Add(Container container, ProductInstance productInstance);
}
