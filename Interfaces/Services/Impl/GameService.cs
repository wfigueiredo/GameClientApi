using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameClientApi.Domain.DTO;
using GameClientApi.Domain.Enum;
using GameClientApi.Domain.Model;
using GameClientApi.Domain.Translators;
using GameClientApi.Interfaces.Clients.Http;
using GameClientApi.Interfaces.Error;
using Hangfire;
using Newtonsoft.Json;

namespace GameClientApi.Interfaces.Services.Impl
{
    public class GameService : IGameService
    {
        private const string FILE_NAME = "GameReleases";

        private readonly IGDBClient _igdbClient;
        private readonly IFileService _fileService;

        public GameService(IGDBClient igdbClient, IFileService fileService)
        {
            _igdbClient = igdbClient;
            _fileService = fileService;
        }

        public async Task<IEnumerable<Game>> fetchGameReleases(DateTime StartDate, DateTime? EndDate, int maxResults)
        {
            if (EndDate < StartDate)
                throw new GenericApiException("EndDate should be greater than StartDate");

            var response = await _igdbClient.FetchGameReleases(StartDate, EndDate, maxResults);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IGDBNextGameReleasesResponse>(content);

            return result.ToDomainList();
        }

        public Task ExportMonthlyReleases(ReportDto reportDto)
        {
            BackgroundJob.Enqueue(() => ProcessExportInBackground(reportDto));
            return Task.CompletedTask;
        }

        public async Task ProcessExportInBackground(ReportDto reportDto)
        {
            var Now = DateTime.Now;
            var StartDate = new DateTime(Now.Year, Now.Month, 1);
            var nextReleases = await fetchGameReleases(StartDate, Now, reportDto.maxResults);
            var filePath = ExportToFile(nextReleases, reportDto.fileExtension);
            await _fileService.UploadToS3(filePath);
        }

        /// <summary>
        /// Exports the desired content to a file, returning the path.
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        public string ExportToFile(IEnumerable<Game> Content, FileExtension fileExtension) => fileExtension switch
        {
            FileExtension.Csv => _fileService.GenerateCsvFile(FILE_NAME, Content),
            _ => throw new InvalidOperationException("Unknown file extension")
        };
    }
}
