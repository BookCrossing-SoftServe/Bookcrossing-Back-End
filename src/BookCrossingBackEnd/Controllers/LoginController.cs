using Application.Dto;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BookCrossingBackEnd.Filters;

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {

        
        private readonly ITokenService _tokenService;

        public LoginController(ITokenService tokenService)
        {
            _tokenService = tokenService;
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


            var user = await _tokenService.VerifyUserCredentials(model);

            var tokenStr = _tokenService.GenerateJSONWebToken(user);

            IActionResult response = Ok(new { token = tokenStr,firstName=user.FirstName,lastName=user.LastName });
            return response;
        }
    }
}
