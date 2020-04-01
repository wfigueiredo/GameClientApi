using System.Collections.Generic;
using System.Threading.Tasks;
using GameProducer.Domain.Enum;
using GameProducer.Domain.Model;
using GameProducer.Interfaces.Clients;
using GameProducer.Interfaces.Error;
using Microsoft.Extensions.Configuration;

namespace GameProducer.Interfaces.Services.Impl
{
    public class PublisherService : IPublisherService
    {
        private readonly SQSClient _sqsClient;
        private readonly SNSClient _snsClient;
        private readonly IConfiguration _config;

        public PublisherService(SQSClient sqsClient, SNSClient snsClient, IConfiguration config)
        {
            _sqsClient = sqsClient;
            _snsClient = snsClient;
            _config = config;
        }

        private string GetTopicName(BasePayload payloadType) => payloadType switch
        {
            Game _ => _config.GetValue<string>("AWS:SNS:Topics:GameReleaseTopic"),
            _ => throw new GenericApiException($"Unknown entity type")
        };

        private string GetQueueName(BasePayload payloadType) => payloadType switch
        {
            Game g => GetConsoleQueueName(g.consoleAbbreviation),
            User _ => _config.GetValue<string>("AWS:SQS:Queues:User"),
            _ => throw new GenericApiException($"Unknown entity type")
        };

        private string GetConsoleQueueName(ConsoleType consoleType) => consoleType switch
        {
            ConsoleType.PS4 => _config.GetValue<string>("AWS:SQS:Queues:PlayStation"),
            ConsoleType.XOne => _config.GetValue<string>("AWS:SQS:Queues:Xbox"),
            ConsoleType.Switch => _config.GetValue<string>("AWS:SQS:Queues:Switch"),
            _ => throw new GenericApiException($"Unknown Console type")
        };

        public async Task publishToQueueAsync<T>(IEnumerable<T> content)
        {
            foreach (var item in content)
            {
                var QueueName = GetQueueName(item as BasePayload);
                await _sqsClient.publishAsync(QueueName, item);
            }
        }

        public async Task publishToTopicAsync<T>(T content)
        {
            var TopicName = GetTopicName(content as BasePayload);
            await _snsClient.publishAsync(TopicName, content);
        }
    }
}
