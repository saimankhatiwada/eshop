using Domain.Abstractions;

namespace Domain.Products;
public sealed record Quantity
{
    public static readonly Error Invalid = new("Quantity.Invalid", "The quantity is invalid");

    private Quantity(int value) => Value = value;

    public int Value { get; init; }

    public static Result<Quantity> Create(int value)
    {
        if (value < 0)
        {
            return Result.Failure<Quantity>(Invalid);
        }

        return new Quantity(value);
    }
}
