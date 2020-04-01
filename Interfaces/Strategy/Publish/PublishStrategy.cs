using GameProducer.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameProducer.Interfaces.Strategy
{
    public interface PublishStrategy
    {
        Task Apply<T>(IEnumerable<T> type);
    }
}
