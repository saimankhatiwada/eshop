using Domain.Products;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder.HasKey(product => product.Id);

        builder.Property(product => product.Name)
            .HasMaxLength(200)
            .HasConversion(name => name.Value, value => new Name(value));

        builder.Property(product => product.Description)
            .HasMaxLength(2000)
            .HasConversion(description => description.Value, value => new Description(value));

        builder.Property(product => product.Quantity)
            .HasConversion(quantity => quantity.Value, value => Quantity.Create(value).Value);

        builder.OwnsOne(product => product.Money, priceBuilder =>
        {
            priceBuilder.Property(money => money.Currency)
                .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
        });
    }
}
