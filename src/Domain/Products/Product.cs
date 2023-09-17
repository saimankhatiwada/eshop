using Domain.Abstractions;
using Domain.Products.Events;
using Domain.Shared;

namespace Domain.Products;

public sealed class Product : Entity
{
    private Product(
        Guid id,
        Name name,
        Description description,
        Money money,
        Quantity quantity)
        :base(id)
    {
        Name = name;
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
    public Description Description { get; private set; }
    public Money Money { get; private set; }
    public Quantity Quantity { get; private set; }

    public static Product Create(
        Name name,
        Description description,
        Money money,
        Quantity quantity)
    {
        var product = new Product(
            Guid.NewGuid(),
            name,
            description,
            money,
            quantity);

        product.RaiseDomainEvent(new ProductCreatedDomainEvent(product.Id));

        return product;
    }
}
