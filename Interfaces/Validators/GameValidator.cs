using GameProducer.Domain.Infrastructure;
using GameProducer.Domain.Model;
using GameProducer.Interfaces.Error;
using GameProducer.Util;
using System;
using System.Threading.Tasks;

namespace GameProducer.Interfaces.Validators
{
    public class GameValidator : IValidator<Game>
    {
        public async Task<Game> Validate(Game game)
        {
            throw new NotImplementedException();
        }
    }
}
