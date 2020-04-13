using GameProducer.Domain.Infrastructure;
using GameProducer.Domain.Model;
using GameProducer.Interfaces.Error;
using GameProducer.Util;

namespace GameProducer.Interfaces.Validators
{
    public class UserValidator : IValidator<User>
    {
        public void Validate(User u, params string[] p)
        {
            if (EnumExtensions.GetDisplayName(DestinationType.Topic) == p[0])
                throw new GenericApiException("Invalid operation destination type. Use 'queue' instead");

            if (!HttpUtil.IsValidEmail(u.emailAddress))
                throw new GenericApiException($"Email [{u.emailAddress}] is not valid");

            if (!ValidationUtil.IsValidPhoneNumber(u.phoneNumber))
                throw new GenericApiException($"Phone number [{u.phoneNumber}] is not valid");
        }
    }
}
