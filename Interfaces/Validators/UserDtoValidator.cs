using GameClientApi.Domain.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameClientApi.Interfaces.Validators.user
{
    public class UserDtoValidator : IValidator<UserDto>
    {
        public Task<UserDto> Validate(UserDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
