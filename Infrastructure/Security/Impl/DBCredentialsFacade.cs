using GameProducer.Infrastructure.Contracts;
using Microsoft.Extensions.Configuration;

namespace GameProducer.Infrastructure.Security.Impl
{
    public class DBCredentialsFacade : ICredentialsFacade<DBCredentials>
    {
        private readonly ISecretsManagerFacade _secretsManager;
        private readonly IConfiguration _config;

        public DBCredentialsFacade(ISecretsManagerFacade secretsManager, IConfiguration config)
        {
            _secretsManager = secretsManager;
            _config = config;
        }

        public DBCredentials GetCredentials()
        {
            return GetFromAppsettings() ?? GetFromSecretsManagerAsync();
        }

        private DBCredentials GetFromAppsettings()
        {
            var postgresConfig = _config.GetSection("PostgreSQL");
            
            return new DBCredentials
            {
                Username = postgresConfig["username"],
                Password = postgresConfig["password"]
            };
        }

        private DBCredentials GetFromSecretsManagerAsync()
        {
            var SecretId = _config.GetValue<string>("SecretsManager");
            return _secretsManager.GetObjectProperty<DBCredentials>(SecretId);
        }
    }
}
