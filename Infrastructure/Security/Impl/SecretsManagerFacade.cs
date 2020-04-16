using GameProducer.Interfaces.Clients.AWS;
using Newtonsoft.Json;

namespace GameProducer.Infrastructure.Security
{
    public class SecretsManagerFacade : ISecretsManagerFacade
    {
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