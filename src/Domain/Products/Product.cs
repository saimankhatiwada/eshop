using Domain.Abstractions;
using Domain.Products.Events;
using Domain.Shared;

namespace Domain.Products;

public sealed class Product : Entity<ProductId>
{
    private Product(
        ProductId id,
        Name name,
        ImageName imageName,
        Description description,
        Money money,
        Quantity quantity)
        :base(id)
    {
        Name = name;
        ImageName = imageName;
        Description = description;
        Money = money;
        Quantity = quantity;
    }

    #pragma warning disable CS8618
    private Product()
    {
    }
    #pragma warning restore CS8618
    public Name Name { get; private set; }
    public ImageName ImageName { get; private set; }
    public Description Description { get; private set; }
    public Money Money { get; private set; }
    public Quantity Quantity { get; private set; }

    public static Product Create(
        Name name,
        ImageName imageName,
        Description description,
        Money money,
        Quantity quantity)
    {
        var product = new Product(
            ProductId.New(),
            name,
            imageName,
            description,
            money,
            quantity);

        product.RaiseDomainEvent(new ProductCreatedDomainEvent(product.Id));

        return product;
    }

    public Result Update(
        Name name,
        ImageName imageName,
        Description description,
        Money money,
        Quantity quantity)
    {
        Name = name;
        ImageName = imageName;
        Description = description;
        Money = money;
        Quantity = quantity;
        RaiseDomainEvent(new ProductUpdatedDomainEvent(Id));
        return Result.Success();
    }

    public Result Delete()
    {
        RaiseDomainEvent(new ProductDeletedDomainEvent(Id));
        return Result.Success();
    }
}
