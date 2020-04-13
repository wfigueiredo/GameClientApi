using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using GameProducer.Infrastructure.Contracts;
using GameProducer.Infrastructure.Security;
using GameProducer.Interfaces.Error;
using GameProducer.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GameProducer.Interfaces.Clients
{
    public class SNSClient
    {
        private readonly ILogger<SNSClient> _logger;
        private readonly ISecretsManagerFacade _secretsManager;
        private readonly AmazonSimpleNotificationServiceClient _client;

        public SNSClient(SecretsManagerFacade secretsManager, ILogger<SNSClient> logger)
        {
            _logger = logger;
            _secretsManager = secretsManager;
            _client = CreateClient();
        }

        private AmazonSimpleNotificationServiceClient CreateClient()
        {
            _logger.LogInformation("Creating SNSClient...");

            var awsCredentials = _secretsManager.GetObjectProperty<AwsCredentials>(SecretsManagerFacade.SECRET_NAME_AWS_CREDENTIALS);
            var BasicAwsCredentials = new BasicAWSCredentials(awsCredentials.AccessKey, awsCredentials.SecretKey);
            var Endpoint = RegionEndpoint.GetBySystemName(awsCredentials.Region);

            _logger.LogInformation("SNS Client created succesfully");

            return new AmazonSimpleNotificationServiceClient(BasicAwsCredentials, Endpoint);
        }

        private async Task<string> GetTopicArnAsync(string topicName)
        {
            var TopicInfo = await _client.FindTopicAsync(topicName);
            return TopicInfo.TopicArn;
        }

        public async Task publishAsync<TMessage>(string TopicName, TMessage payload)
        {
            if (payload == null)
                throw new ArgumentNullException(nameof(payload));

            var TopicArn = await GetTopicArnAsync(TopicName);
            var MessageBody = JsonConvert.SerializeObject(payload);
            var result = await _client.PublishAsync(TopicArn, MessageBody);

            if (!HttpUtil.IsSuccessStatusCode(result.HttpStatusCode))
            {
                _logger.LogError($"Could not publish to {TopicName}");
                throw new GenericApiException($"Could not publish to {TopicName}");
            }
        }
    }
}
