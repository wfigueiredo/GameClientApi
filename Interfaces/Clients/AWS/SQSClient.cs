using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using GameProducer.Interfaces.Error;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GameProducer.Interfaces.Clients
{
    public class SQSClient
    {
        private readonly ILogger<SQSClient> _logger;
        private readonly IConfiguration _config;
        private readonly AmazonSQSClient _client;

        public SQSClient(IConfiguration configuration, ILogger<SQSClient> logger)
        {
            _logger = logger;
            _config = configuration;
            _client = CreateClient();
        }

        private AmazonSQSClient CreateClient()
        {
            _logger.LogInformation("Creating SQSClient...");

            var credentials = _config.GetSection("AWS");
            var AwsCredentials = new BasicAWSCredentials(credentials["AccessKey"], credentials["SecretKey"]);
            var Endpoint = RegionEndpoint.GetBySystemName(credentials["Region"]);
            
            _logger.LogInformation("SQS Client created succesfully");
            
            return new AmazonSQSClient(AwsCredentials, Endpoint);
        }

        public async Task publishAsync<TMessage>(string QueueName, TMessage payload)
        {
            if (payload == null) throw new ArgumentNullException(nameof(payload));

            var QueueUrl = ComposeQueueUri(QueueName).AbsoluteUri;
            var MessageRequest = new SendMessageRequest(QueueUrl, JsonConvert.SerializeObject(payload));

            try
            {
                await _client.SendMessageAsync(MessageRequest);
                _logger.LogInformation($"Successfully published message at queue at {QueueUrl}");
            }
            catch (Exception ex)
            {
                var ErrorMessage = $"Failed to publish message at queue at {QueueUrl}";
                _logger.LogError(ErrorMessage);
                throw new GenericApiException(ErrorMessage, ex.Message);
            }
        }

        private Uri ComposeQueueUri(string QueueName)
        {
            var sqsConfig = _config.GetSection("AWS");
            var Endpoint = RegionEndpoint.GetBySystemName(sqsConfig["Region"]).SystemName;
            var AccountId = sqsConfig["AccountId"];
            return new Uri($"https://sqs.{Endpoint}.amazonaws.com/{AccountId}/{QueueName}");
        }
    }
}
