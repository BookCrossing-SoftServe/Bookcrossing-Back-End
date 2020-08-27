using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.Password;
using Application.Dto.QueryParams;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private IUserService _userService;
        private IUserResolverService _userResolverService;
        private readonly ILogger _logger;

        public UsersController(IUserService userService, IUserResolverService userResolverService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _userResolverService = userResolverService;
            _logger = logger;
        }

        /// <summary>
        /// Get list of all users (only for admin)
        /// </summary>
        /// <returns></returns>
        // GET: api/<controller>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Getting all users");
            var users = await _userService.GetAllUsers();
            if (users == null) return NoContent();
            return Ok(users);
        }


        [HttpGet("paginated")]
        [Authorize]
        public async Task<ActionResult<PaginationDto<UserDto>>> GetAllUsers([FromQuery] FullPaginationQueryParams fullPaginationQuery)
        {
            _logger.LogInformation("Getting all paginated users");
            return await _userService.GetAllUsers(fullPaginationQuery);
        }

        /// <summary>
        /// Get user by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<controller>/5
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDto>> Get([FromRoute] int userId)
        {
            _logger.LogInformation("Getting user by id = {userId}", userId);
            var user = await _userService.GetById(x => x.Id == userId);
            if (user == null)
                return NotFound();
            return user;
        }

        // GET api/<controller>/5
        [HttpGet("id")]
        [Authorize]
        public async Task<ActionResult<int>> GetUserId()
        {
            _logger.LogInformation("Getting user id");
            var userId = _userResolverService.GetUserId();
            return Ok(userId);
        }

        // PUT api/<controller>/5
        /// <summary>
        /// Function for updating info about user
        /// </summary>
        /// <param name="user"></param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody]UserUpdateDto user)
        {
            _logger.LogInformation("Updating user {id}", id);
            if (id == _userResolverService.GetUserId() || _userResolverService.IsUserAdmin())
            {
                user.Id = id;
                await _userService.UpdateUser(user);
                return NoContent();
            }
            _logger.LogWarning("Update user NOT FOUND");
            return Forbid();
        }

        /// <summary>
        /// Function for deleting user (only for admin)
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            _logger.LogInformation("Delete user {id}", id);
            await _userService.RemoveUser(id);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RecoverUser([FromRoute] int id)
        {
            _logger.LogInformation("Recover user {id}", id);
            await _userService.RecoverDeletedUser(id);
            return Ok();
        }

        [HttpPost("password")]
        public async Task<IActionResult> ForgotPassword([FromBody]ResetPasswordDto email)
        {
            await _userService.SendPasswordResetConfirmation(email.Email);
            return Ok();
        }

        [HttpPut("password")]
        public async Task<IActionResult> CreateNewPassword([FromBody]ResetPasswordDto newPassword)
        {
            await _userService.ResetPassword(newPassword);
            return Ok();
        }

        [HttpPut("email")]
        public async Task<IActionResult> ForbidEmailNotification([FromBody]ForbidEmailDto email)
        {
            _logger.LogInformation("Forbid email notififcations for {email}", email);
            if (await _userService.ForbidEmailNotification(email))
                return Ok();
            _logger.LogWarning("Forbid email NOT FOUND");
            return BadRequest();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<RegisterDto>> Register([FromBody] RegisterDto user)
        {
            _logger.LogInformation("Create user {user}", user);
            var createdUser = await _userService.AddUser(user);
            if (createdUser != null)
            {
                return CreatedAtAction(nameof(GetUserId), new { id = createdUser.Id }, createdUser);
            }
            _logger.LogWarning("User already EXISTS");
            return BadRequest();
        }
    }
}
