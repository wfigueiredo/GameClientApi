
using GameProducer.Domain.Enum;
using System;

namespace GameProducer.Domain.Model
{
    public class Game : BasePayload
    {
        public override string type { get; set; } = "game";
        public long externalId { get; set; }
        public string name { get; set; }
        public string summary { get; set; }
        public string publisher { get; set; }
        public DateTime releaseDate { get; set; }
        public ConsoleType consoleType { get; set; }
        public string consoleAbbreviation { get; set; }
        public string consoleName { get; set; }
    }
}
