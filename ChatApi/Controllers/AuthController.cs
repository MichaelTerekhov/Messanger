using Messanger.Bll.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messanger.Bll.Contracts;
using Messanger.Bll.Contracts.User;

namespace ChatApi.Controllers
{
    /// <summary>
    /// This controller contains authorization operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        private readonly ILogger<AuthController> _logger;

        private IConfiguration Configuration { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authService">Auth service</param>
        /// <param name="logger">Logger</param>
        /// <param name="configuration">Configuration</param>
        public AuthController(
            IAuthService authService,
            ILogger<AuthController> logger,
            IConfiguration configuration)
        {
            _authService = authService;
            _logger = logger;
            Configuration = configuration;
            _authService.SecurityKey = Configuration.GetSection("AuthKey").GetValue<string>("Secret");
        }

        /// <summary>
        /// Registration action
        /// </summary>
        /// <param name="registerDto">Requested dto for registration on platform</param>
        /// <returns>Returns JWT</returns>
        [HttpPost("register")]
        public async Task<ActionResult<string>> RegisterAsync([FromBody] UserRegisterDto registerDto)
        {
            if (!ModelState.IsValid || registerDto == null)
            {
                return BadRequest();
            }

            try
            {
                var token = await _authService.RegisterAsync(registerDto);
                return CheckTokenAndReturn(token, "Register failed!");
            }
            catch (ArgumentNullException e)
            {
                _logger.LogWarning("Argument null exception. " + e.ParamName);
                return BadRequest();
            }
        }

        /// <summary>
        /// Login action
        /// </summary>
        /// <param name="loginDto">Requested dto for login on platform</param>
        /// <returns>Returns JWT</returns>
        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginAsync([FromBody] UserLoginDto loginDto)
        {
            if (!ModelState.IsValid || loginDto == null)
            {
                return BadRequest();
            }

            try
            {
                var token = await _authService.LoginAsync(loginDto);
                return CheckTokenAndReturn(token, "Login failed!");
            }
            catch (ArgumentNullException e)
            {
                _logger.LogWarning(e.Message);
                return BadRequest();
            }
        }

        private ActionResult<string> CheckTokenAndReturn(string jwt, string message)
        {
            if (!string.IsNullOrEmpty(jwt))
                return Ok(jwt);

            if (!string.IsNullOrEmpty(message))
                _logger.LogWarning(message);

            return BadRequest("Authorization failed");
        }
    }
}
