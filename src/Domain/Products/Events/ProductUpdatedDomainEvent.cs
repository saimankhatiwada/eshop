using Domain.Abstractions;

namespace Domain.Products.Events;
public sealed record ProductUpdatedDomainEvent(ProductId ProductId) : IDomainEvent;