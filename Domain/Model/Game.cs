
using GameClientApi.Domain.Enum;
using System;

namespace GameClientApi.Domain.Model
{
    public class Game
    {
        public long externalId { get; set; }
        public string name { get; set; }
        public string consoleAbbreviation { get; set; }
        public string consoleName { get; set; }
        public string publisher { get; set; }
        public DateTime releaseDate { get; set; }
        public string summary { get; set; }
        public ConsoleType consoleType { get; set; }
    }
}
