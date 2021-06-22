using Messanger.Bll.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Messanger.Bll.Contracts.Permissions;
using Messanger.Bll.Contracts.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace ChatApi.Controllers
{
    /// <summary>
    /// This controller contains operations with user accounts.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;

        private readonly IAccountService _accountService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accountService">Account service</param>
        /// <param name="logger">Logger</param>
        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        /// <summary>
        /// Get all accounts for admin
        /// </summary>
        /// <returns>All accounts</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AccountViewDto>>> GetAllAccountsForChat()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            if (accounts == null)
                return NotFound();

            return Ok(accounts);
        }
    }
}
