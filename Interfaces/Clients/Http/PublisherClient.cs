using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PublisherApi.Interfaces.Clients.Http
{
    public class PublisherClient
    {
        private readonly IConfiguration _config;
        private readonly ILogger<PublisherClient> _logger;
        private readonly HttpClient _HttpClient;
        private readonly IHttpClientFactory _HttpClientFactory;

        public PublisherClient(IConfiguration config, ILogger<PublisherClient> logger, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _logger = logger;
            _HttpClientFactory = httpClientFactory;

            var ClientName = _config.GetValue<string>("Integration:NotificationApi:ClientName");
            _HttpClient = _HttpClientFactory.CreateClient(ClientName);
        }

        public async Task<HttpResponseMessage> PublishMessage<TMessage>(TMessage message)
        {
            _logger.LogInformation($"Starting HTTP call to NotificationApi...");
            
            var Json = JsonConvert.SerializeObject(message);
            var Content = new StringContent(Json, Encoding.UTF8, "application/json");

            var integrationSection = _config.GetSection("Integration:NotificationApi");
            var Address = $"{integrationSection["Host"]}{integrationSection["Endpoint"]}";

            var response = await _HttpClient.PostAsync(new Uri(Address), Content);
            response.EnsureSuccessStatusCode();

            _logger.LogInformation($"Integration successful with NotificationApi");

            return response;
        }
    }
}
