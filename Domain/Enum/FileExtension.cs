using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameClientApi.Domain.Enum
{
    public enum FileExtension
    {
        [Display(Name = "csv")]
        Csv,

        [Display(Name = "json")]
        Json
    }
}
