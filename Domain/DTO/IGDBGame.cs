using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameProducer.Domain.DTO
{
    public class IGDBGame
    {
        public long id { get; set; }
        public IEnumerable<IGDBInvolvedCompany> involved_companies { get; set; }
        public string name { get; set; }
        public string summary { get; set; }
    }
}
