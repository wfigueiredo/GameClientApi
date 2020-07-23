using CsvHelper;
using GameClientApi.Domain.Enum;
using GameClientApi.Infrastructure.Extensions;
using GameClientApi.Mappers;
using GameClientApi.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GameClientApi.Interfaces.Services.Impl
{
    public class FileService : IFileService
    {
        private const string REPORTS_PATH = "./././reports/";

        public Task GenerateCsvFile<T>(string fileName, IEnumerable<T> Content)
        {
            var timeStampedFileName = $"{DateTimeUtil.ConvertToString(DateTime.Now, DateTimeUtil.DATE_TIME_TIMESTAMPED_LABEL_FORMAT)}_{fileName}";
            var fullPath = $"{REPORTS_PATH}{timeStampedFileName}.{FileExtension.Csv.GetDisplayName()}";

            using (var writer = new StreamWriter(fullPath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Configuration.RegisterClassMap<GameMap>();
                csv.WriteRecords(Content);
            }

            return Task.CompletedTask;
        }
    }
}
