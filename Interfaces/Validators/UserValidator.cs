using GameProducer.Domain.Infrastructure;
using GameProducer.Domain.Model;
using GameProducer.Interfaces.Error;
using GameProducer.Util;
using System.Threading.Tasks;

namespace GameProducer.Interfaces.Validators
{
    public class UserValidator : IValidator<User>
    {
        public async Task<User> Validate(User user)
        {
            if (!HttpUtil.IsValidEmail(user.emailAddress))
                throw new GenericApiException($"Email [{user.emailAddress}] is not valid");

            if (!ValidationUtil.IsValidPhoneNumber(user.phoneNumber))
                throw new GenericApiException($"Phone number [{user.phoneNumber}] is not valid");

            await Task.CompletedTask;

            return user;
        }
    }
}
