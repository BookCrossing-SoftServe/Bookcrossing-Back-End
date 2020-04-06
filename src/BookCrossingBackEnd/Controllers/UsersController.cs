using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Dto;
using BookCrossingBackEnd.Filters;
using Domain.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]



    public class UsersController : Controller
    {

        private IUser UserService { get; set; }

        public UsersController(IUser userService)
        {
            this.UserService = userService;
       
        }

        /// <summary>
        /// Get list of all users (only for admin)
        /// </summary>
        /// <returns></returns>
        // GET: api/<controller>
        [HttpGet]
        public async Task<ActionResult<UserDto>> Get()
        {
            var users = await UserService.GetAllUsers();
            return Ok(users);
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
            throw new NotImplementedException();
        }


        [HttpPost]
        public async Task<IActionResult> Creste([FromBody])



        // PUT api/<controller>/5
        /// <summary>
        /// Function for updating info about user
        /// </summary>
        /// <param name="user"></param>
        [HttpPut]
        [Authorize]
       // [UserUpdateFilter]
        public async Task<IActionResult> Update([FromBody]UserUpdateDto user)
        {
            
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity?.FindFirst(ClaimTypes.Name)?.Value;
            user.Id = int.Parse(userId);
            await UserService.UpdateUser(user);
            return Ok();
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
            var userId = claimsIdentity?.FindFirst(ClaimTypes.Name)?.Value;
            await UserService.RemoveUser(int.Parse(userId));
            return Ok();
        }
    }
}
