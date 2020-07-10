using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameClientApi.Interfaces.Services.Impl
{
    public interface ILoginService
    {
        string generateJwtToken(int Id, string role);
    }
}
