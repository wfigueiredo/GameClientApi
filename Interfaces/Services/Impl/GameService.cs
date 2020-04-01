using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GameProducer.Domain.DTO;
using GameProducer.Domain.Model;
using GameProducer.Domain.Translators;
using GameProducer.Interfaces.Clients.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Extensions.Http;

namespace GameProducer.Interfaces.Services.Impl
{
    public class GameService : IGameService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<GameService> _logger;
        private readonly IGDBClient _igdbClient;
        private int RetryAttemptInterval => _config.GetValue<int>("RetryPolicies:HttpTransientError:Timespan");

        public GameService(IConfiguration config, ILogger<GameService> logger, IGDBClient igdbClient)
        {
            _config = config;
            _logger = logger;
            _igdbClient = igdbClient;
        }

        public async Task<IEnumerable<Game>> fetchWeekGameReleases()
        {
            var response = await BuildTransientHttpErrorRetryPolicy()
            .ExecuteAsync(async() =>
            {
                return await _igdbClient.FetchNextReleaseDates();
            });

            var Content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IGDBNextGameReleasesResponse>(Content);
            
            return result.ToDomainList();
        }

        private IAsyncPolicy<HttpResponseMessage> BuildTransientHttpErrorRetryPolicy()
        {
            return HttpPolicyExtensions
              .HandleTransientHttpError()
              .OrResult(r => !r.IsSuccessStatusCode)
              .WaitAndRetryForeverAsync(
                sleepDurationProvider: retryAttempt => TimeSpan.FromMinutes(RetryAttemptInterval),
                onRetry: (result, timeSpan, context) =>
                {
                    _logger.LogError($"Integration error on fetch operation from IGDB API.");
                    _logger.LogWarning($"Next attempt starting in {RetryAttemptInterval} minutes...");
                });
        }
    }
}
