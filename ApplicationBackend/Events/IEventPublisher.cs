using Domain.Topics;
using Shared.ResultPattern;

namespace ApplicationBackend.Events;

public interface IEventPublisher
{
    Task<Result<TMessage>> PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken) where TMessage : class;
}