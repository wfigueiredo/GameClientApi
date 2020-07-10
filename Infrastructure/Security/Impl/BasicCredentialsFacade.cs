using Microsoft.Extensions.Configuration;
using SecretsManagerFacadeLib.Contracts;
using SecretsManagerFacadeLib.Interfaces;

namespace GameProducer.Infrastructure.Security.Impl
{
    public class BasicCredentialsFacade : ICredentialsFacade<BasicCredentials>
    {
        private readonly ISecretsManagerFacade _secretsManager;
        private readonly IConfiguration _config;

        public BasicCredentialsFacade(ISecretsManagerFacade secretsManager, IConfiguration config)
        {
            _secretsManager = secretsManager;
            _config = config;
        }

        public BasicCredentials GetCredentials()
        {
            return GetFromAppsettings() ?? GetFromSecretsManagerAsync();
        }

        private BasicCredentials GetFromAppsettings()
        {
            var postgresConfig = _config.GetSection("PostgreSQL");
            
            return new BasicCredentials
            {
                Username = postgresConfig["username"],
                Password = postgresConfig["password"]
            };
        }

        private BasicCredentials GetFromSecretsManagerAsync()
        {
            var SecretId = _config.GetValue<string>("SecretsManager");
            return _secretsManager.GetObjectProperty<BasicCredentials>(SecretId);
        }
    }
}
