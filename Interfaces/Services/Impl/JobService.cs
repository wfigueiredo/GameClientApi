using GameProducer.Util;
using Hangfire;
using Hangfire.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameProducer.Interfaces.Services.Impl
{
    public class JobService : IJobService
    {
        private ILogger<JobService> _logger;
        private IConfiguration _config;
        private readonly IGameService _gameService;
        private readonly IPublisherService _publisherService;

        public JobService(ILogger<JobService> logger, IConfiguration config, IGameService gameService, IPublisherService publisherService)
        {
            _logger = logger;
            _config = config;
            _gameService = gameService;
            _publisherService = publisherService;
        }

        public void RegisterRecurringJobs()
        {
            RegisterPublishNextGameReleases();
        }

        private bool IsRecurringJobRegistered(string jobId)
        {
            using (var connection = JobStorage.Current.GetConnection())
            {
                IEnumerable<string> registeredRecurringJobIds =
                    from job in StorageConnectionExtensions.GetRecurringJobs(connection)
                    select job.Id;

                return registeredRecurringJobIds.Contains(jobId);
            }
        }

        public void RegisterPublishNextGameReleases()
        {
            var section = _config.GetSection("RecurringJobs:PublishNextGameReleases");
            var jobId = section["Id"];
            var jobCron = section["Cron"];

            if (!IsRecurringJobRegistered(jobId))
            {
                _logger.LogInformation($"Registering {jobId} recurring job...");
                RecurringJob.AddOrUpdate(jobId, () => publishScheduledContent(), jobCron, DateTimeUtil.TARGET_TIMEZONE);
                _logger.LogInformation("Registering complete");
            }
        }

        public async Task publishScheduledContent()
        {
            var nextReleases = await _gameService.fetchWeekGameReleases();
            await _publisherService.publishToQueueAsync(nextReleases);
        }
    }
}
