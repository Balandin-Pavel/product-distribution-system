using System.Diagnostics;
using ProductDistribution.Containers;
using ProductDistribution.Containers.Results;
using ProductDistribution.Distribution.Results;
using ProductDistribution.Products;

namespace ProductDistribution.Distribution.Rules;

public sealed class ContainerDistributionRule : IDistributionRule
{
    private readonly Container _container;
    private readonly IContainerService _containerService;

    public ContainerDistributionRule(Container container, IContainerService containerService)
    {
        _container = container;
        _containerService = containerService;
    }

    public DistributionResult Distribute(ProductInstance productInstance)
    {
        ContainerAddResult result = _containerService.Add(_container, productInstance);

        return result switch
        {
            ContainerAddResult.Success => new DistributionResult.Success(),

            ContainerAddResult.Failure => new DistributionResult.Failure(),

            _ => throw new UnreachableException("Container service returned an unknown result"),
        };
    }
}
