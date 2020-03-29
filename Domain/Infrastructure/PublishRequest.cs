namespace GameProducer.Domain.Infrastructure
{
    public class PublishRequest<T>
    {
        public T content { get; set; }
        public Metadata metadata { get; set; }
    }
}
