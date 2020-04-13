using System.Collections.Generic;

namespace GameProducer.Domain.Infrastructure
{
    public class PublishRequest<T>
    {
        public IEnumerable<T> content { get; set; }
        public ModelMetadata metadata { get; set; }
    }
}
