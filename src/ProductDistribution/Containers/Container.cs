using ProductDistribution.Products;

namespace ProductDistribution.Containers;

public sealed class Container
{
    public Container(ContainerId containerId, Ovh maximumOvh)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(maximumOvh, new Ovh(0, 0));

        Id = containerId;
        MaximumOvh = maximumOvh;
    }

    public ContainerId Id { get; }

    public Ovh MaximumOvh { get; }
}
