namespace Application.Users.Shared;

#pragma warning disable CS8618
public sealed record UserResponse
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string ImageName { get; init; }
}

#pragma warning restore CS8618