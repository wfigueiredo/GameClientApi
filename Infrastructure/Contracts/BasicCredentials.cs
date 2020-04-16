using System.Runtime.Serialization;

namespace GameProducer.Infrastructure.Contracts
{
    [DataContract]
    public class BasicCredentials
    {
        [DataMember(Name = "username")]
        public string Username { get; set; }

        [DataMember(Name = "password")]
        public string Password { get; set; }
    }
}
