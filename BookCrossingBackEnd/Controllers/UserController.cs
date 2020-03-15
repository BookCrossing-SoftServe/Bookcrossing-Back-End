using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IServices;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        public UserController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }
        [Route("GetProfile")]
        [HttpGet]
        public IActionResult GetUserProfile(int userId)
        {
            var user = _userProfileService.GetMyProfile(userId);
            return Ok(user);
        }
        [Route("AddNewBook")]
        [HttpPost]
        public IActionResult AddNewBook(Book book)
        {
            _userProfileService.AddNewBook(book);
            return Ok(book);
        }
    }
}
