using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameClientApi.Interfaces.Services
{
    public interface IFileService
    {
        string GenerateCsvFile<T>(string fileName, IEnumerable<T> Content);
        Task UploadToS3(string path);
    }
}
