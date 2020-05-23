using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class UserTokenDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public TokenDto Token { get; set; }
    }
}
