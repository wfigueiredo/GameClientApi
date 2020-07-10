using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GameProducer.Domain.Enum;
using GameProducer.Domain.Model;
using GameProducer.Interfaces.Repository;
using GameProducer.Util;
using Microsoft.IdentityModel.Tokens;

namespace GameProducer.Interfaces.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<User> FindAll()
        {
            return _repository.FindAll();
        }

        public IEnumerable<User> FindUsers(RoleType roleType) => roleType switch
        {
            RoleType.Root => FindAll(),
            RoleType.Admin => FindAll().Where(user => user.role != RoleType.Root)
        };

        public User FindById(int Id)
        {
            return _repository.FindById(Id);
        }

        public User FindByUsernameAndSecret(string username, string secret)
        {
            return _repository.FindByUsernameAndSecretAsync(username, secret);
        }
    }
}
