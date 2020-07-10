using GameProducer.Domain.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameProducer.Interfaces.Validators.user
{
    public class UserDtoValidator : IValidator<UserDto>
    {
        public Task<UserDto> Validate(UserDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
