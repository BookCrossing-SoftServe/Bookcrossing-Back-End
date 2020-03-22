using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {

        private IUser Service { get; set; }
        private IConfiguration Configuration { get; set; }
        private IToken TokenService { get; set; }

        public LoginController(IUser userService, IConfiguration configuration, IToken tokenService)
        {
            this.Service = userService;
            this.Configuration = configuration;
            this.TokenService = tokenService;
        }




        /// <summary>
        /// Function for user authentication.
        /// </summary>
        /// <param name="model">This parameter receives email and password from form on client side</param>
        /// <returns>Returns JSON web token or http response code 401(Unauthorized)</returns>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginDto model)
        {
            IActionResult response = Unauthorized();
            

            var user = await Service.VerifyUserCredentials(model);


            if (user == null) return response;

            var tokenStr = TokenService.GenerateJSONWebToken(user);

            //var tokenStr = TokenService.GenerateJSONWebToken(user);
            response = Ok(new { token = tokenStr });
            return response;
        }
    }
}
