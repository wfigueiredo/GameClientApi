using GameProducer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameProducer.Interfaces.Repository
{
    public interface IUserRepository
    {
        User FindByUsernameAndSecretAsync(string username, string secret);
        IEnumerable<User> FindAll();
        User FindById(int id);
    }
}
