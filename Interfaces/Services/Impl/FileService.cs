using CsvHelper;
using GameClientApi.Domain.Enum;
using GameClientApi.Infrastructure.Extensions;
using GameClientApi.Interfaces.Clients.Aws;
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
        private readonly S3Client _s3Client;

        public FileService(S3Client s3Client)
        {
            _s3Client = s3Client;
        }

        public string GenerateCsvFile<T>(string fileName, IEnumerable<T> Content)
        {
            var timeStampedFileName = $"{DateTimeUtil.ConvertToString(DateTime.Now, DateTimeUtil.DATE_TIME_TIMESTAMPED_LABEL_FORMAT)}_{fileName}";
            var filePath = $"{REPORTS_PATH}{timeStampedFileName}.{FileExtension.Csv.GetDisplayName()}";

            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Configuration.RegisterClassMap<GameMap>();
                csv.WriteRecords(Content);
            }

            return filePath;
        }

        public async Task UploadToS3(string path)
        {
            if (!File.Exists(path))
                throw new ArgumentNullException(path);

            await _s3Client.UploadFile(path);
        }
    }
}
