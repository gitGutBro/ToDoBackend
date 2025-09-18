using ApplicationBackend.Events;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Shared.ResultPattern;

namespace InfrastructureBackend.Messaging;

public class MassTransitEventPublisher(IServiceProvider serviceProvider) : IEventPublisher
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public async Task<Result<TMessage>> PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken) where TMessage : class
    {
        ITopicProducer<TMessage>? producer = _serviceProvider.GetService<ITopicProducer<TMessage>>();

        if (producer is null)
            return Result<TMessage>.Failure(Error.NullReference);

        await producer.Produce(message, cancellationToken);

        return Result<TMessage>.Success(message);
    }
}