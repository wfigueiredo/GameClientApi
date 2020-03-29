using GameProducer.Domain.Model;
using GameProducer.Interfaces.Error;
using GameProducer.Util;

namespace GameProducer.Interfaces.Validators
{
    public class UserValidator : IValidator<User>
    {
        public void Validate(User u, params string[] p)
        {
            if (HttpUtil.IsValidEmail(u.Email))
                throw new GenericApiException($"The Email [{u.Email}] is not valid");
        }
    }
}
