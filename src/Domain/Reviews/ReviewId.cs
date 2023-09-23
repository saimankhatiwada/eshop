namespace Domain.Reviews;

public record ReviewId(Guid value)
{
    public static ReviewId New() => new(Guid.NewGuid());
}
