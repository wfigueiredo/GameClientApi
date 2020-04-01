using GameProducer.Domain.Infrastructure;
using GameProducer.Domain.Model;
using GameProducer.Interfaces.Error;
using GameProducer.Interfaces.Strategy;
using GameProducer.Interfaces.Validators;
using GameProducer.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
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
                var User = Enumerable.ToList(publishRequest.content).FirstOrDefault();
                _validator.Validate(User, publishRequest.metadata.destinationType.GetDisplayName());
                await _publishStrategyContext.Apply(publishRequest);
                return StatusCode(StatusCodes.Status202Accepted, new { message = "Message(s) in transit" });
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
