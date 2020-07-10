using GameClientApi.Domain.DTO.User;
using GameClientApi.Domain.Enum;
using GameClientApi.Domain.Infrastructure;
using GameClientApi.Domain.Model;
using GameClientApi.Domain.Translators;
using GameClientApi.Infrastructure.Extensions;
using GameClientApi.Interfaces.Error;
using GameClientApi.Interfaces.Services;
using GameClientApi.Interfaces.Services.Impl;
using GameClientApi.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GameClientApi.Controllers
{
    [Authorize]
    [Route("api/publisherapi/v1")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly ILoginService _loginService;

        public UserController(ILogger<UserController> logger, IUserService userService, ILoginService loginService)
        {
            _logger = logger;
            _userService = userService;
            _loginService = loginService;
        }

        [AllowAnonymous]
        [HttpPost("user/authenticate")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Authenticate([FromBody] UserDto credentials)
        {
            try
            {
                var user = _userService.FindByUsernameAndSecret(credentials.username, credentials.secret);
                if (user == null)
                    return NotFound(new { message = "Invalid username or password" });

                return Ok(new UserDto()
                {
                    username = user.username,
                    role = user.role,
                    token = _loginService.generateJwtToken(user.Id, user.role.GetDisplayName())
                });
            }
            catch (GenericApiException ex)
            {
                _logger.LogError(ex.Message);
                return Unauthorized(new { message = "Operation error", reason = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal error on Authentication process", reason = ex.Message });
            }
        }

        [AuthorizeRoles(RoleTypeDesc.Root, RoleTypeDesc.Admin)]
        [HttpGet("users")]
        [Produces("application/json")]
        public async Task<IActionResult> FindAll()
        {
            try
            {
                var userId = int.Parse(User.Identity.Name);
                var user = _userService.FindById(userId);
                //bool isRoot = User.IsInRole(RoleTypeDesc.Root);

                var userList = _userService.FindUsers(user.role);
                if (!userList.Any())
                    return NotFound();

                return Ok(userList.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal error on User list", reason = ex.Message });
            }
        }

        [AuthorizeRoles(RoleTypeDesc.Root, RoleTypeDesc.Admin)]
        [HttpPost("user")]
        [Consumes("application/json")]
        public async Task<IActionResult> Create([FromBody] UserDto dto)
        {
            try
            {
                return Created("api/publisherapi/v1/user", dto);
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
