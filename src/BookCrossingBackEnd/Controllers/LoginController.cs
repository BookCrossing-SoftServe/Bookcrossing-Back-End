using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using BookCrossingBackEnd.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {

        private readonly ITokenService _tokenService;
        private readonly IUserResolverService _userResolverService;
        public LoginController(ITokenService tokenService, IUserResolverService userResolverService)
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
            var jwt = _tokenService.GenerateJWT(claims);
            var refreshToken = await _tokenService.GenerateRefreshToken(user);


            UserTokenDto userTokenDto = new UserTokenDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = new TokenDto() { JWT = jwt, RefreshToken = refreshToken }
            };

            return Ok(userTokenDto);
        }

        [HttpPost("refresh")]
        [LoginFilter]
        public async Task<ActionResult<UserTokenDto>> Refresh([FromBody] TokenDto refreshTokenModel)
        {
            var refresh = await _tokenService.VerifyRefreshToken(refreshTokenModel.RefreshToken);

            var claims = _tokenService.GetPrincipalFromExpiredToken(refreshTokenModel.JWT);
            var jwt = _tokenService.GenerateJWT(claims.Claims);
            var refreshToken = await _tokenService.UpdateRefreshRecord(refresh);

            UserTokenDto userToken = new UserTokenDto
            {
                Id = refresh.User.Id,
                FirstName = refresh.User.FirstName,
                LastName = refresh.User.LastName,
                Token = new TokenDto { JWT = jwt, RefreshToken = refreshToken }
            };
            return Ok(userToken);
        }


    }
}
