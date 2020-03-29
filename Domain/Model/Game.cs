
using GameProducer.Domain.Enum;

namespace GameProducer.Domain.Model
{
    public class Game : BasePayload
    {
        public string title { get; set; }
        public ConsoleType console { get; set; }
        public string publisher { get; set; }
    }
}
