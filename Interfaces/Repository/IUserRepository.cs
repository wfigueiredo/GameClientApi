using GameClientApi.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameClientApi.Interfaces.Repository
{
    public interface IUserRepository
    {
        User FindByUsernameAndSecretAsync(string username, string secret);
        IEnumerable<User> FindAll();
        User FindById(int id);
    }
}
