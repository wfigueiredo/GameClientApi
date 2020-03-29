using GameProducer.Domain.Infrastructure;
using GameProducer.Interfaces.Error;
using GameProducer.Interfaces.Services;
using System.Threading.Tasks;

namespace GameProducer.Interfaces.Strategy
{
    public class PublishStrategyContext
    {
        private readonly IPublisherService _publisherService;

        public PublishStrategyContext(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        public Task Apply<T>(PublishRequest<T> publishRequest) => publishRequest.metadata.destinationType switch
        { 
            DestinationType.Queue => _publisherService.publishToQueueAsync(publishRequest.content),
            DestinationType.Topic => _publisherService.publishToTopicAsync(publishRequest.content),
            _ => throw new GenericApiException($"Unknown destination type")
        };
    }
}
