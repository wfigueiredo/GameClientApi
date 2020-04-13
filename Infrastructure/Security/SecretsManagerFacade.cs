using GameProducer.Interfaces.Clients.AWS;
using Newtonsoft.Json;

namespace GameProducer.Infrastructure.Security
{
    public class SecretsManagerFacade : ISecretsManagerFacade
    {
        public const string SECRET_NAME_AWS_CREDENTIALS = "SecretsManager.gpapi_aws";
        public const string SECRET_NAME_RDS_CREDENTIALS = "SecretsManager.gpapi_rds";
        public const string SECRET_NAME_IGDB_API_KEY = "SecretsManager.gpapi_igdb";
        
        private readonly SecretsManagerClient _client;
        

        public SecretsManagerFacade(SecretsManagerClient client)
        {
            _client = client;
    }
        public string GetStringProperty(string secretId)
        {
            return _client.GetSecret(secretId, x => x).Result;
        }

        public T GetObjectProperty<T>(string secretId)
        {
            return _client.GetSecret(secretId, JsonConvert.DeserializeObject<T>).Result;
        }
    }
}