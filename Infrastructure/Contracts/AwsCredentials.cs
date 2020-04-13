using System.Runtime.Serialization;

namespace GameProducer.Infrastructure.Contracts
{
    [DataContract]
    public class AwsCredentials
    {
        [DataMember(Name = "accountid")]
        public string AccountId { get; set; }

        [DataMember(Name = "accesskey")]
        public string AccessKey { get; set; }
        
        [DataMember(Name = "secretkey")]
        public string SecretKey { get; set; }
        
        [DataMember(Name = "region")]
        public string Region { get; set; }
    }
}
