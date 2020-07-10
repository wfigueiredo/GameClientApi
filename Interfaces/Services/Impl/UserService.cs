using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GameClientApi.Domain.Enum;
using GameClientApi.Domain.Model;
using GameClientApi.Interfaces.Repository;
using GameClientApi.Util;
using Microsoft.IdentityModel.Tokens;

namespace GameClientApi.Interfaces.Services.Impl
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
