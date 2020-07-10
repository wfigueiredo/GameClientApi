using System.Collections.Generic;
using System.Linq;
using GameProducer.Domain.Enum;
using GameProducer.Domain.Model;
using GameProducer.Util;

namespace GameProducer.Interfaces.Repository.Impl
{
    public class UserRepository : IUserRepository
    {
        private IEnumerable<User> _users;

        public UserRepository()
        {
            _users = BuildUserList();
        }

        public IEnumerable<User> BuildUserList()
        {
            var Root = new User()
            {
                Id = 1,
                name = "Seraphites",
                username = "seraphites",
                secret = HashUtil.SHA1("s3r4ph1t35"),
                role = RoleType.Root
            };

            var Admin = new User()
            {
                Id = 2,
                name = "Fireflies",
                username = "fireflies",
                secret = HashUtil.SHA1("f1r3fl135"),
                role = RoleType.Admin
            };

            var User1 = new User()
            {
                Id = 3,
                name = "Washington Liberation Front",
                username = "wlf",
                secret = HashUtil.SHA1("wlf2020"),
                role = RoleType.User
            };

            var User2 = new User()
            {
                Id = 4,
                name = "Hunters",
                username = "hunters",
                secret = HashUtil.SHA1("hunt3r5"),
                role = RoleType.User
            };

            yield return Root;
            yield return Admin;
            yield return User1;
            yield return User2;
        }

        public IEnumerable<User> FindAll()
        {
            return _users;
        }

        public User FindById(int id)
        {
            return _users.SingleOrDefault(user => user.Id == id);
        }

        public User FindByUsernameAndSecretAsync(string username, string secret)
        {
            return _users
                .SingleOrDefault(user => user.username == username
                    && user.secret == HashUtil.SHA1(secret));
        }
    }
}
