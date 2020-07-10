using GameClientApi.Domain.DTO.User;
using GameClientApi.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameClientApi.Domain.Translators
{
    public static class UserTranslator
    {
        public static UserDto ToDto(this User user)
        {
            if (user != null)
            {
                return new UserDto()
                {
                    username = user.username,
                    role = user.role
                };
            }

            return null;
        }

        public static IList<UserDto> ToDto(this IEnumerable<User> users)
        {
            return users?.Select(ToDto).ToList();
        }
    }
}
