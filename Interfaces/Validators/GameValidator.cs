using GameProducer.Domain.Infrastructure;
using GameProducer.Domain.Model;
using GameProducer.Interfaces.Error;
using GameProducer.Util;

namespace GameProducer.Interfaces.Validators
{
    public class GameValidator : IValidator<Game>
    {
        public void Validate(Game g, params string[] p)
        {
            if (EnumUtil.GetDisplayName(DestinationType.Topic) == p[0] &&
                !string.IsNullOrEmpty(g.consoleType.GetDisplayName()))
            {
                throw new GenericApiException("Invalid operation destination type. Use 'queue' instead");
            }
        }
    }
}
