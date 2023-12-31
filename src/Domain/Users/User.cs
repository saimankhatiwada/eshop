using System.Net.Mime;
using Domain.Abstractions;
using Domain.Users.Events;

namespace Domain.Users;

public sealed class User : Entity<UserId>
{
    private User(
        UserId id,
        FirstName firstName,
        LastName lastName,
        Email email,
        ImageName imageName)
        :base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        ImageName = imageName;
    }


    #pragma warning disable CS8618
    private User()
    {
    }
    #pragma warning restore CS8618

    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public Email Email { get; private set; }
    public ImageName ImageName { get; private set; }
    public string IdentityId { get; private set; } = string.Empty;

    public static User Create(FirstName firstName, LastName lastName, Email email, ImageName imageName)
    {
        User user = new User(UserId.New(), firstName, lastName, email, imageName);

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

        return user;
    }

    public void SetIdentityId(string identityId)
    {
        IdentityId = identityId;
    }
}
