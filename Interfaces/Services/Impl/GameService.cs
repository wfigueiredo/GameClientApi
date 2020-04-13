using System.Collections.Generic;
using System.Threading.Tasks;
using GameProducer.Domain.DTO;
using GameProducer.Domain.Model;
using GameProducer.Domain.Translators;
using GameProducer.Interfaces.Clients.Http;
using Newtonsoft.Json;

namespace GameProducer.Interfaces.Services.Impl
{
    public class GameService : IGameService
    {
        private readonly IGDBClient _igdbClient;

        public GameService(IGDBClient igdbClient)
        {
            _igdbClient = igdbClient;
        }

        public async Task<IEnumerable<Game>> fetchWeekGameReleases()
        {
            var response = await _igdbClient.FetchNextReleaseDates();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IGDBNextGameReleasesResponse>(content);

            return result.ToDomainList();
        }
    }
}
