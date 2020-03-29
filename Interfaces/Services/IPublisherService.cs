using GameProducer.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameProducer.Interfaces.Services
{
    public interface IPublisherService
    {
        Task publishToTopicAsync<T>(T request);
        Task publishToQueueAsync<T>(T request);
    }
}
