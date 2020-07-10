using System.Collections.Generic;

namespace GameProducer.Domain.Infrastructure
{
    public class PublishRequest<T>
    {
        public string groupId { get; set; }
        public DestinationType destinationType { get; set; }
        public IEnumerable<T> content { get; set; }
    }
}
