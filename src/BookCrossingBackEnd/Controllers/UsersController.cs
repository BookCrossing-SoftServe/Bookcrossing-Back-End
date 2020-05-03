using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Dto.Password;
using AutoMapper;
using BookCrossingBackEnd.Filters;
using Application.Dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]



    public class UsersController : Controller
    {
        private IUserService UserService { get; set; }
        private IUserResolverService UserResolverService { get; set; }

        public UsersController(IUserService userService, IUserResolverService userResolverService)
        {
            this.UserService = userService;
            this.UserResolverService = userResolverService;
        }

        /// <summary>
        /// Get list of all users (only for admin)
        /// </summary>
        /// <returns></returns>
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await UserService.GetAllUsers();
            if (users == null) return NoContent();
            return Ok(users);
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
            var user = await UserService.GetById(x=>x.Id == userId);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        // GET api/<controller>/5
        [HttpGet("id")]
        [Authorize]
        public async Task<ActionResult<int>> GetUserId()
        {
            var userId = UserResolverService.GetUserId();
            return Ok(userId);
        }




        // PUT api/<controller>/5
        /// <summary>
        /// Function for updating info about user
        /// </summary>
        /// <param name="user"></param>
        [HttpPut("{id}")]
        [Authorize]
        //[UserUpdateFilter]
        public async Task<IActionResult> Update([FromRoute] int id,[FromBody]UserUpdateDto user)
        {
            if (id == UserResolverService.GetUserId() || UserResolverService.IsUserAdmin())
            {
                user.Id = id;
                await UserService.UpdateUser(user);
                return NoContent();
            }

            return Forbid();
        }

        /// <summary>
        /// Function for deleting user (only for admin)
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/<controller>/5
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity?.FindFirst("id")?.Value;
            await UserService.RemoveUser(int.Parse(userId));
            return Ok();
        }
        [HttpPost("password")]
        [AllowAnonymous]
        [ModelValidationFilter]
        public async Task<IActionResult> ForgotPassword([FromBody]ResetPasswordDto email)
        {
            await UserService.SendPasswordResetConfirmation(email.Email);
            return Ok();
        }

        [HttpPut("password")]
        [AllowAnonymous]
        [ModelValidationFilter]
        public async Task<IActionResult> CreateNewPassword([FromBody]ResetPasswordDto newPassword)
        {
            await UserService.ResetPassword(newPassword);
            return Ok();
        }
    }
}
