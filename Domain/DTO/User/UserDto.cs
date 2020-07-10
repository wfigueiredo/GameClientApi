using GameClientApi.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameClientApi.Domain.DTO.User
{
    public class UserDto
    {
        public string username { get; set; }
        public string secret { get; set; }
        public RoleType role { get; set; }
        public string token { get; set; }
    }
}
