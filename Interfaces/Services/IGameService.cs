using GameProducer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameProducer.Interfaces.Services
{
    public interface IGameService
    {
        Task<IEnumerable<Game>> fetchGameReleases(DateTime StartDate, DateTime EndDate);
    }
}
