using MediatR;

namespace CalculadoraCedears.Api.Domian.Events
{
    public class CedearEventHandler :
        INotificationHandler<CedearHasBeenInserted>
    {
        public Task Handle(CedearHasBeenInserted notification, CancellationToken cancellationToken)
        {
            //You do something when the Sample is created.
            //I highly recommend adding a messaging system like RabbitMQ, Azure Service Bus, or Amazon EventBridge.
            return Task.CompletedTask;
        }
    }
}
