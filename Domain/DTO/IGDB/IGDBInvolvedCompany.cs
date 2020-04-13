using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameProducer.Domain.DTO
{
    public class IGDBInvolvedCompany
    {
        public long id { get; set; }
        public IGDBCompany company { get; set; }
        public bool publisher { get; set; }
    }
}
