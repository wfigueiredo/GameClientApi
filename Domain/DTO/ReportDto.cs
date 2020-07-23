using GameClientApi.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameClientApi.Domain.DTO
{
    public class ReportDto
    {
        public FileExtension fileExtension { get; set; }
        public int maxResults { get; set; }
    }
}
