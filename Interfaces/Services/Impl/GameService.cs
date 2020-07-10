﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameClientApi.Domain.DTO;
using GameClientApi.Domain.Model;
using GameClientApi.Domain.Translators;
using GameClientApi.Interfaces.Clients.Http;
using GameClientApi.Interfaces.Error;
using Newtonsoft.Json;

namespace GameClientApi.Interfaces.Services.Impl
{
    public class GameService : IGameService
    {
        private readonly IGDBClient _igdbClient;

        public GameService(IGDBClient igdbClient)
        {
            _igdbClient = igdbClient;
        }

        public async Task<IEnumerable<Game>> fetchGameReleases(DateTime StartDate, DateTime EndDate)
        {
            if (EndDate < StartDate)
                throw new GenericApiException("EndDate should be greater than StartDate");

            var response = await _igdbClient.FetchGameReleases(StartDate, EndDate);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IGDBNextGameReleasesResponse>(content);

            return result.ToDomainList();
        }
    }
}
