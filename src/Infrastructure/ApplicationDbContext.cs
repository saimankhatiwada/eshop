using Application.Abstractions.Clock;
using Application.Exceptions;
using Domain.Abstractions;
using Infrastructure.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Infrastructure;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    private readonly IPublisher _publisher;
    private readonly IDateTimeProvider _dateTimeProvider;
    public ApplicationDbContext(
        DbContextOptions options, 
        IPublisher publisher,
        IDateTimeProvider dateTimeProvider)
        : base(options)
    {
        _publisher = publisher;
        _dateTimeProvider = dateTimeProvider;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            AddDomainEventsAsOutboxMessage();

            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concurrency exception occurred.", ex);
        }
    }

    private void AddDomainEventsAsOutboxMessage()
    {
        var domainEvents = ChangeTracker
            .Entries<IEntity>()
            .Select(entity => entity.Entity)
            .SelectMany(entity => 
            {
                var domainEvents = entity.GetDomainEvents();

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage(
                Guid.NewGuid(),
                _dateTimeProvider.UtcNow,
                domainEvent.GetType().Name,
                JsonConvert.SerializeObject(domainEvent, JsonSerializerSettings)))
            .ToList();

            AddRange(domainEvents);
    }
}
