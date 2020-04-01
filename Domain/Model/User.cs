
namespace GameProducer.Domain.Model
{
    public class User : BasePayload
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }
}
