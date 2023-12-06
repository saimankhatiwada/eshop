using Application.Abstractions.Messaging;
using Application.Users.Shared;

namespace Application.Users.GetUser;

public sealed record GetUserQuery(Guid UserId) : IQuery<UserResponse>;