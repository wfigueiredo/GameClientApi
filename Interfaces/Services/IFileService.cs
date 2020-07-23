using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameClientApi.Interfaces.Services
{
    public interface IFileService
    {
        Task GenerateCsvFile<T>(string fileName, IEnumerable<T> Content);
    }
}
