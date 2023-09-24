using System.Data;
using Application.Abstractions.Clock;
using Application.Abstractions.Data;
using Dapper;
using Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;

namespace Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxMessagesJob : IJob
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IPublisher _publisher;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly OutboxOptions _outboxOptions;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger;

    public ProcessOutboxMessagesJob(
        ISqlConnectionFactory sqlConnectionFactory,
        IPublisher publisher,
        IDateTimeProvider dateTimeProvider,
        IOptions<OutboxOptions> outboxOptions,
        ILogger<ProcessOutboxMessagesJob> logger)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _publisher = publisher;
        _dateTimeProvider = dateTimeProvider;
        _outboxOptions = outboxOptions.Value;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Beginning to process outbox messages");

        using var connection = _sqlConnectionFactory.CreateConnection();
        using var transaction = connection.BeginTransaction();

        var outboxMessages = await GetOutboxMessagesAsync(connection, transaction);

        foreach (var outboxMessage in outboxMessages)
        {
            Exception? exception = null;

            try
            {
                var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                    outboxMessage.Content,
                    JsonSerializerSettings)!;

                await _publisher.Publish(domainEvent);
            }
            catch (Exception caughtException)
            {
                _logger.LogError(
                    caughtException,
                    "Exception while processing outbox message {MessageId}",
                    outboxMessage.Id);

                exception = caughtException;
            }

            await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception);
        }

        transaction.Commit();

        _logger.LogInformation("Completed processing outbox messages");
    }

    private async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessagesAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction)
    {
        string sql = $"""
            SELECT 
                id,
                content
            FROM outbox_messages
            WHERE processed_on_utc IS NULL
            ORDER BY occured_on_utc
            LIMIT {_outboxOptions.BatchSize}
            FOR UPDATE
            """;

        var outboxMessages = await dbConnection.QueryAsync<OutboxMessageResponse>(sql, dbTransaction);

        return outboxMessages.ToList();
    }

    private async Task UpdateOutboxMessageAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        OutboxMessageResponse outboxMessage,
        Exception? exception)
    {
        const string sql = @"
            UPDATE outbox_messages
            SET processed_on_utc = @ProcessedOnUtc,
                error = @Error
            WHERE id = @Id";

        await dbConnection.ExecuteAsync(
            sql,
            new
            {
                outboxMessage.Id,
                ProcessedOnUtc = _dateTimeProvider.UtcNow,
                Error = exception?.ToString()
            },
            transaction: dbTransaction);
    }

    internal sealed record OutboxMessageResponse(Guid Id, string Content);
}
