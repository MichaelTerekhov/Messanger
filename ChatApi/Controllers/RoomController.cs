using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messanger.Bll.Contracts.Rooms;
using Messanger.Bll.Services;
using Microsoft.AspNetCore.Authorization;

namespace ChatApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;

        private readonly IRoomService _roomService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accountService">Account service</param>
        /// <param name="logger">Logger</param>
        public RoomController(IRoomService roomService, ILogger<AccountController> logger)
        {
            _roomService = roomService;
            _logger = logger;
        }

        /// <summary>
        /// Get all accounts for admin
        /// </summary>
        /// <returns>All accounts</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<RoomViewDto>>> GetAllRoomsAvailableForYou()
        {
            var userIdFromToken = ClaimsIdentityService.GetIdFromToken(User);
            var rooms = await _roomService.GetAllAvailableRoomsForYouAsync(userIdFromToken);
            if (rooms == null)
                return NotFound();

            return Ok(rooms);
        }
    }
}

