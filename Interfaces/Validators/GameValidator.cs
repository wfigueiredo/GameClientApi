using GameClientApi.Domain.Infrastructure;
using GameClientApi.Domain.Model;
using GameClientApi.Interfaces.Error;
using GameClientApi.Util;
using System;
using System.Threading.Tasks;

namespace GameClientApi.Interfaces.Validators
{
    public class GameValidator : IValidator<Game>
    {
        public async Task<Game> Validate(Game game)
        {
            throw new NotImplementedException();
        }
    }
}
