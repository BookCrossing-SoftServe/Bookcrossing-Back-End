using Application.Dto;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BookCrossingBackEnd.Filters;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {

        private readonly ITokenService _tokenService;
        private readonly IUserResolverService _userResolverService;
        public LoginController(ITokenService tokenService,IUserResolverService userResolverService)
        {
            _tokenService = tokenService;
            _userResolverService = userResolverService;
        }

        /// <summary>
        /// Function for user authentication.
        /// </summary>
        /// <param name="model">This parameter receives email and password from form on client side</param>
        /// <returns>Returns JSON web token or http response code 401(Unauthorized)</returns>
        [HttpPost]
        [LoginFilter]
        public async Task<ActionResult<UserTokenDto>> Login([FromBody]LoginDto model)
        {
            var user = await _tokenService.VerifyUserCredentials(model);

            var claims = new[]
            {
                new Claim("id",user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role,user.Role.Name)
            };
            var tokenDto = await _tokenService.GenerateTokens(user, claims); ;

            UserTokenDto userTokenDto = new UserTokenDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenDto
            };
           
            return Ok(userTokenDto);
        }

        [HttpPost("refresh")]
        [LoginFilter]
        public async Task<ActionResult<UserTokenDto>> Refresh([FromBody] TokenDto refreshToken)
        {
            var user = await _tokenService.VerifyRefreshToken(refreshToken.RefreshToken);
            if (user.Id != _userResolverService.GetUserId()) return Forbid();
            
            var claims = _tokenService.GetPrincipalFromExpiredToken(refreshToken.JWT);
            var tokens = await _tokenService.GenerateTokens(user, claims.Claims);

            UserTokenDto userToken = new UserTokenDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokens
            };
            return Ok(userToken);
        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public IActionResult Get()
        {
            IActionResult actionResult = new ObjectResult(_userResolverService.GetClaims());
            return actionResult;
        }

    }
}
