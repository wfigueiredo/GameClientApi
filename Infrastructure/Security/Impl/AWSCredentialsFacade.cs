using GameProducer.Infrastructure.Contracts;
using GameProducer.Infrastructure.Security;
using Microsoft.Extensions.Configuration;

namespace GameProducer.Interfaces.Clients.AWS
{
    public class AWSCredentialsFacade : ICredentialsFacade<AwsCredentials>
    {
        private readonly ISecretsManagerFacade _secretsManager;
        private readonly IConfiguration _config;

        public AWSCredentialsFacade(ISecretsManagerFacade secretsManager, IConfiguration config)
        {
            _secretsManager = secretsManager;
            _config = config;
        }

        public AwsCredentials GetCredentials()
        {
            return GetFromAppsettings() ?? GetFromSecretsManagerAsync();
        }

        private AwsCredentials GetFromSecretsManagerAsync()
        {
            var SecretId = _config.GetValue<string>("AWS:SecretsManager");
            return _secretsManager.GetObjectProperty<AwsCredentials>(SecretId);
        }

        private AwsCredentials GetFromAppsettings()
        {
            var section = _config.GetSection("AWS:Credentials");
            
            if (section.Exists())
            {
                return new AwsCredentials
                {
                    AccountId = section["AccountId"],
                    AccessKey = section["AccessKey"],
                    SecretKey = section["SecretKey"],
                    Region = section["Region"]
                };
            }

            return null;
        }
    }
}
