using GameProducer.Domain.Infrastructure;
using GameProducer.Domain.Model;
using GameProducer.Interfaces.Error;
using GameProducer.Interfaces.Strategy;
using GameProducer.Interfaces.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GameProducer.Controllers
{
    [Route("")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly PublishStrategyContext _publishStrategyContext;
        private readonly IValidator<User> _validator;

        public UserController(ILogger<UserController> logger, PublishStrategyContext publishStrategyContext, IValidator<User> validator)
        {
            _logger = logger;
            _publishStrategyContext = publishStrategyContext;
            _validator = validator;
        }

        [HttpPost("user/publish")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> PublishInfo([FromBody] PublishRequest<User> publishRequest)
        {
            try
            {
                _validator.Validate(publishRequest.content);
                await _publishStrategyContext.Apply(publishRequest);
                return StatusCode(StatusCodes.Status202Accepted, new { message = "Message in transit" });
            }
            catch (GenericApiException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "Invalid request", reason = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal error on Publisher API", reason = ex.Message });
            }
        }
    }
}
