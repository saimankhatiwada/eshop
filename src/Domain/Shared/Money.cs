namespace Domain.Shared;

public record Money(decimal Amount, Currency Currency)
{
    public static Money operator +(Money first, Money secound)
    {
        if(first.Currency != secound.Currency)
        {
            throw new InvalidOperationException("Currencies has to be equal");
        }

        return new Money(first.Amount + secound.Amount, first.Currency);
    }
    
    public static Money Zero() => new(0, Currency.None);
}
