using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameProducer.Infrastructure.Security
{
    public interface ISecretsManagerFacade
    {
        string GetStringProperty(string secretName);
        T GetObjectProperty<T>(string secretName);
    }
}
