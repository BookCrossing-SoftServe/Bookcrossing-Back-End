using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IServices;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [Route("Users")]
        [HttpGet]
        public List<User> GetListOfUsers()
        {
            return _adminService.ListAllUsers().ToList();
        }
        [Route("Genres")]
        [HttpGet]
        public List<Genre> GetListOfGenres()
        {
            return _adminService.ListAllGenres().ToList();
        }
        [Route("Users")]
        [HttpDelete]
        public IActionResult RemoveUser(int userId)
        {
            try
            {
                _adminService.RemoveUser(userId);
                return Ok("Successfully deleted");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Bad user id!");
            }
        }
    }
}
