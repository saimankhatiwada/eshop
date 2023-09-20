using Domain.Abstractions;

namespace Domain.Products.Events;
public sealed record ProductDeletedDomainEvent(Guid ProductId) : IDomainEvent;