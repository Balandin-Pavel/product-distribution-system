namespace ProductDistribution.Products;

public sealed record Sku
{
    public Sku(SkuId id, Ovh ovh, CategoryId categoryId)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(ovh, new Ovh(0, 0));

        Id = id;
        CategoryId = categoryId;
        Ovh = ovh;
    }

    public SkuId Id { get; }

    public CategoryId CategoryId { get; }

    public Ovh Ovh { get; }
}
