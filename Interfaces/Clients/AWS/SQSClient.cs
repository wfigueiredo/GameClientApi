using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using GameProducer.Infrastructure.Contracts;
using GameProducer.Infrastructure.Security;
using GameProducer.Interfaces.Error;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GameProducer.Interfaces.Clients
{
    public class SQSClient
    {
        private readonly ILogger<SQSClient> _logger;
        private readonly AmazonSQSClient _client;
        private readonly ISecretsManagerFacade _secretsManager;

        public SQSClient(SecretsManagerFacade secretsManager, ILogger<SQSClient> logger)
        {
            _logger = logger;
            _secretsManager = secretsManager;
            _client = CreateClient();
        }

        private AmazonSQSClient CreateClient()
        {
            _logger.LogInformation("Creating SQSClient...");

            var awsCredentials = _secretsManager.GetObjectProperty<AwsCredentials>(SecretsManagerFacade.SECRET_NAME_AWS_CREDENTIALS);
            var BasicAwsCredentials = new BasicAWSCredentials(awsCredentials.AccessKey, awsCredentials.SecretKey);
            var Endpoint = RegionEndpoint.GetBySystemName(awsCredentials.Region);
            
            _logger.LogInformation("SQS Client created succesfully");
            
            return new AmazonSQSClient(BasicAwsCredentials, Endpoint);
        }

        public async Task publishAsync<TMessage>(string QueueName, TMessage payload)
        {
            if (payload == null) 
                throw new ArgumentNullException(nameof(payload));

            var queueUrl = ComposeQueueUri(QueueName).AbsoluteUri;
            var messageBody = JsonConvert.SerializeObject(payload);
            var messageRequest = new SendMessageRequest(queueUrl, messageBody);

            if (queueUrl.EndsWith(".fifo"))
            {
                messageRequest.MessageGroupId = "publisherapi";
            }

            try
            {
                await _client.SendMessageAsync(messageRequest);
                _logger.LogInformation($"Successfully published message at queue at {queueUrl}");
            }
            catch (Exception ex)
            {
                var ErrorMessage = $"Failed to publish message at queue at {queueUrl}";
                _logger.LogError(ErrorMessage);
                throw new GenericApiException(ErrorMessage, ex.Message);
            }
        }

        private Uri ComposeQueueUri(string QueueName)
        {
            var awsCredentials = _secretsManager.GetObjectProperty<AwsCredentials>(SecretsManagerFacade.SECRET_NAME_AWS_CREDENTIALS);
            var Endpoint = RegionEndpoint.GetBySystemName(awsCredentials.Region).SystemName;
            return new Uri($"https://sqs.{Endpoint}.amazonaws.com/{awsCredentials.AccountId}/{QueueName}");
        }
    }
}
