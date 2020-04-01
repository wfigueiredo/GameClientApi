using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameProducer.Interfaces.Services
{
    public interface IPublisherService
    {
        Task publishToTopicAsync<T>(T request);
        Task publishToQueueAsync<T>(IEnumerable<T> content);
    }
}
