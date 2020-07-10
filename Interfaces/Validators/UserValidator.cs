using GameClientApi.Domain.Infrastructure;
using GameClientApi.Domain.Model;
using GameClientApi.Interfaces.Error;
using GameClientApi.Util;
using System.Threading.Tasks;

namespace GameClientApi.Interfaces.Validators
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
