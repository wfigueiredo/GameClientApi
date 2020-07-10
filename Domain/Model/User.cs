using GameProducer.Domain.Enum;

namespace GameProducer.Domain.Model
{
    public class User : BasePublishPayload
    {
        public override string type { get; set; } = "user";

        public int Id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string secret { get; set; }

        public string emailAddress { get; set; }
        
        public string phoneNumber { get; set; }

        public RoleType role { get; set; }
        public int roleId { get; set; }
    }
}
