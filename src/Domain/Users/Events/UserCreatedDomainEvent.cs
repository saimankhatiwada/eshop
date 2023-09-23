using Domain.Abstractions;

namespace Domain.Users.Events;

public sealed record UserCreatedDomainEvent(UserId UserId) : IDomainEvent;