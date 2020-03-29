using GameProducer.Domain.Infrastructure;
using GameProducer.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameProducer.Interfaces.Strategy
{
    public class TopicPublishStrategy : PublishStrategy
    {
        private IPublisherService _publisherService;

        public TopicPublishStrategy(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        public async Task Apply<T>(T content)
        {
            await _publisherService.publishToTopicAsync(content);
        }
    }
}
