namespace ProductDistribution.Products;

public sealed class ProductInstance
{
    public ProductInstance(ProductInstanceId productInstanceId, Sku sku, DateTimeOffset? expirationDate = null)
    {
        Id = productInstanceId;
        Sku = sku;
        ExpirationDate = expirationDate;
    }

    public ProductInstanceId Id { get; }

    public Sku Sku { get; }

    public DateTimeOffset? ExpirationDate { get; }
}
