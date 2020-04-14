using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto.Password;
using AutoMapper;
using BookCrossingBackEnd.Filters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]



    public class UsersController : Controller
    {

        private IUserService _userService { get; set; }
        private IConfiguration configuration { get; set; }
        private ITokenService tokenService { get; set; }

        public UsersController(IUserService userService, IConfiguration configuration, ITokenService tokenService)
        {
            this._userService = userService;
            this.configuration = configuration;
            this.tokenService = tokenService;
        }

        /// <summary>
        /// Get list of all users (only for admin)
        /// </summary>
        /// <returns></returns>
        // GET: api/<controller>
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var IdClaim = User.Claims.FirstOrDefault(x => x.Type.Equals("id", StringComparison.CurrentCultureIgnoreCase));
            if (IdClaim != null)
                return Ok($"Your id is {IdClaim.Value}");
            return BadRequest();

        }




        /// <summary>
        /// Get user by id (only for admin)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Get(int id)
        {
            return Ok("Lol");
        }



        // PUT api/<controller>/5
        /// <summary>
        /// Function for updating info about user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public void Update(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Function for deleting user (only for admin)
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
        [HttpPost("password")]
        [AllowAnonymous]
        [ValidationFilter]
        public async Task<IActionResult> ForgotPassword([FromBody]ResetPasswordDto email)
        {
            await _userService.SendPasswordResetConfirmation(email.Email);
            return Ok();
        }

        [HttpPut("password")]
        [AllowAnonymous]
        [ValidationFilter]
        public async Task<IActionResult> CreateNewPassword([FromBody]ResetPasswordDto newPassword)
        {
            await _userService.ResetPassword(newPassword);
            return Ok();
        }
    }
}
