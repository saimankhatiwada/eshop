namespace Domain.Users;

public record UserId(Guid value)
{
    public static UserId New() => new(Guid.NewGuid());
}
