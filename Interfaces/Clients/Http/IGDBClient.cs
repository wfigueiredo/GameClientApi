using GameProducer.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GameProducer.Interfaces.Clients.Http
{
    public class IGDBClient
    {
        private readonly IConfiguration _config;
        private readonly ILogger<IGDBClient> _logger;
        private readonly HttpClient _HttpClient;
        private readonly IHttpClientFactory _HttpClientFactory;
        private readonly string IGDB_BASE_ADDRESS;

        public IGDBClient(IConfiguration config, ILogger<IGDBClient> logger, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _logger = logger;
            _HttpClientFactory = httpClientFactory;

            IGDB_BASE_ADDRESS = _config.GetValue<string>("Integration:IGDB:Host");
            var ClientName = _config.GetValue<string>("Integration:IGDB:ClientName");
            _HttpClient = _HttpClientFactory.CreateClient(ClientName);
        }

        public async Task<HttpResponseMessage> FetchGameReleases(DateTime StartDate, DateTime? EndDate)
        {
            _logger.LogInformation($"Starting to fetch IGDB Api...");
            var IGDBSection = _config.GetSection("Integration:IGDB");

            var Body = BuildRequestBody(StartDate, EndDate);
            var Address = $"{IGDB_BASE_ADDRESS}{IGDBSection["EndpointReleaseDates"]}";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(Address),
                Content = new StringContent(Body, Encoding.UTF8, "application/apicalypse"),
            };

            var response = await _HttpClient.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            _logger.LogInformation($"Successfully fetched information from IGDB API");

            return response;
        }

        private string BuildRequestBody(DateTime StartDate, DateTime? EndDate)
        {
            var To = EndDate ?? DateTime.Now;
            var UnixTimeStampFrom = DateTimeUtil.FromDateTimeToUnixTimeStamp(StartDate);
            var UnixTimeStampTo = DateTimeUtil.FromDateTimeToUnixTimeStamp(To);

            var IGDBSection = _config.GetSection("Integration:IGDB");
            var Query = $"fields game, game.name, game.summary, game.involved_companies.publisher, " +
                $"game.involved_companies.company.name, date, human, platform.name, platform.abbreviation; " +
                $"where platform = ({IGDBSection["ExternalIds:PlayStation"]},{IGDBSection["ExternalIds:Xbox"]},{IGDBSection["ExternalIds:Switch"]}) & " +
                $"date >= {UnixTimeStampFrom} & date < {UnixTimeStampTo} & " +
                $"category = {IGDBSection["ExternalIds:ConsolePlatform"]}; " +
                $"sort date asc;" +
                $"limit {IGDBSection["MaxResults"]};";
            
            return Query;
        }
    }
}
