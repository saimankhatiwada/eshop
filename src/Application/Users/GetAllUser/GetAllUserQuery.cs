using Application.Abstractions.Messaging;
using Application.Users.Shared;

namespace Application.Users.GetAllUser;

public sealed record GetAllUserQuery() : IQuery<IReadOnlyList<UserResponse>>;
