using GameProducer.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameProducer.Interfaces.Strategy
{
    public class QueuePublishStrategy : PublishStrategy
    {
        private readonly IPublisherService _publisherService;

        public QueuePublishStrategy(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        public async Task Apply<T>(IEnumerable<T> content)
        {
            await _publisherService.publishToQueueAsync(content);
        }
    }
}
