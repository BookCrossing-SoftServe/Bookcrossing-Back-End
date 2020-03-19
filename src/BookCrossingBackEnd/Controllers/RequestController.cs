using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequest _requestService;
        private readonly IToken _tokenService;
        public RequestController(IRequest requestService, IToken tokenService)
        {
            _requestService = requestService;
            _tokenService = tokenService;
        }
        [Route("{bookId}")]
        [HttpGet]
        public IActionResult Request(int bookId)
        {
            var user = new User() {
                FirstName = "Roman",
                MiddleName = "Ferents",
                LastName = "Andriyovych",
                Email = "ferencrman@gmail.com",
                Password = "password",
                RoleId = 1
            };
            _tokenService.GenerateJSONWebToken(user);
            var id = int.Parse(this.User.Claims.First(i => i.Type == "id").Value);
            _requestService.MakeRequest(id, bookId);
                return Ok(id);
        }
        [Route("{bookId}/All")]
        [HttpGet]
        public IActionResult All(int bookId)
        {
            return Ok(_requestService.BookRequests(bookId));
        }
        [Route("{bookId}/Apply")]
        [HttpGet]
        public IActionResult Apply(int requestId)
        {
            _requestService.ApplyRequest(requestId);
            return Ok("Successfully applied");
        }

    }
}
