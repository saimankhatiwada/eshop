using Domain.Abstractions;

namespace Domain.Products.Events;
public sealed record ProductDeletedDomainEvent(ProductId ProductId) : IDomainEvent;