using ProductDistribution.Products;

namespace ProductDistribution.Containers.Results;

public abstract record ContainerAddResult
{
    public sealed record Success : ContainerAddResult;

    public abstract record Failure : ContainerAddResult;

    public sealed record ProductNeighborhoodViolation(ProductInstance ExistingProduct, ProductInstance IncomingProduct) : Failure;

    public sealed record InsufficientCapacity(Ovh Capacity, Ovh StoredOvh, Ovh RequestedOvh) : Failure;
}
