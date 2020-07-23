using CsvHelper.Configuration;
using GameClientApi.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameClientApi.Mappers
{
    public sealed class GameMap : ClassMap<Game>
    {
        public GameMap()
        {
            Map(x => x.externalId)
                .Index(0)
                .Name("externalId");

            Map(x => x.name)
                .Index(1)
                .Name("name");

            Map(x => x.consoleAbbreviation)
                .Index(2)
                .Name("consoleAbbreviation");

            Map(x => x.consoleName)
                .Index(3)
                .Name("consoleName");

            Map(x => x.publisher)
                .Index(4)
                .Name("publisher");

            Map(x => x.releaseDate)
                .Index(5)
                .Name("releaseDate");

            Map(x => x.summary)
                .Index(6)
                .Name("summary");
        }
    }
}
