using Application.Dto;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using BookCrossingBackEnd.Filters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {

        private IUser UserService { get; set; }
        private IConfiguration Configuration { get; set; }
        private IToken TokenService { get; set; }

        public LoginController(IUser userService, IConfiguration configuration, IToken tokenService)
        {
            this.UserService = userService;
            this.Configuration = configuration;
            this.TokenService = tokenService;
        }




        /// <summary>
        /// Function for user authentication.
        /// </summary>
        /// <param name="model">This parameter receives email and password from form on client side</param>
        /// <returns>Returns JSON web token or http response code 401(Unauthorized)</returns>
        [HttpPost]
        [LoginFilter]
        public async Task<IActionResult> Login([FromBody]LoginDto model)
        {


            var user = await UserService.VerifyUserCredentials(model);

            var tokenStr = TokenService.GenerateJSONWebToken(user);

            IActionResult response = Ok(new { token = tokenStr,firstName=user.FirstName,lastName=user.LastName });
            return response;
        }
    }
}
