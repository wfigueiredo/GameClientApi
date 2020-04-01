
using System;
using System.Threading.Tasks;
using GameProducer.Domain.Infrastructure;
using GameProducer.Domain.Model;
using GameProducer.Interfaces.Error;
using GameProducer.Interfaces.Services;
using GameProducer.Interfaces.Strategy;
using GameProducer.Interfaces.Validators;
using GameProducer.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameProducer.Controllers
{
    [Route("")]
    public class GameController : Controller
    {
        private readonly ILogger<GameController> _logger;
        private readonly PublishStrategyContext _publishStrategyContext;
        private readonly IValidator<Game> _validator;
        private readonly IGameService _gameService;

        public GameController(ILogger<GameController> logger, 
            PublishStrategyContext publishStrategyContext,
            IValidator<Game> validator,
            IGameService gameService)
        {
            _logger = logger;
            _validator = validator;
            _gameService = gameService;
            _publishStrategyContext = publishStrategyContext;
        }

        [HttpPost("game/publish")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> PublishInfo([FromBody] PublishRequest<Game> publishRequest)
        {
            try
            {
                var nextReleases = await _gameService.fetchWeekGameReleases();
                return Ok(nextReleases);
                //_validator.Validate(publishRequest.content, publishRequest.metadata.destinationType.GetDisplayName());
                //await _publishStrategyContext.Apply(publishRequest);
                //return StatusCode(StatusCodes.Status202Accepted, new { message = "Message in transit" });
            }
            catch (GenericApiException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "Invalid request",  reason = ex.Message});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal error on Publisher API", reason =  ex.Message});
            }
        }
    }
}
