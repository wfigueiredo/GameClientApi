namespace GameProducer.Domain.Model
{
    public class User : BasePayload
    {
        public override string type { get; set; } = "user";
        public string name { get; set; }

        public string emailAddress { get; set; }
        
        public string phoneNumber { get; set; }
    }
}
