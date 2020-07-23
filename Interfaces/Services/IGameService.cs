using GameClientApi.Domain.DTO;
using GameClientApi.Domain.Enum;
using GameClientApi.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameClientApi.Interfaces.Services
{
    public interface IGameService
    {
        Task<IEnumerable<Game>> fetchGameReleases(DateTime StartDate, DateTime? EndDate, int maxResults);
        Task ExportMonthlyReleases(ReportDto reportDto);
    }
}
