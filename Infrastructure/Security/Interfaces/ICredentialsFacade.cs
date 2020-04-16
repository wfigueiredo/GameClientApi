using GameProducer.Infrastructure.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameProducer.Infrastructure.Security
{
    public interface ICredentialsFacade<T>
    {
        T GetCredentials();
    }
}
