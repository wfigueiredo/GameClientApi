using GameProducer.Domain.Enum;
using GameProducer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameProducer.Interfaces.Services
{
    public interface IUserService
    {
        User FindById(int Id);
        User FindByUsernameAndSecret(string username, string password);
        IEnumerable<User> FindUsers(RoleType roleType);
    }
}
