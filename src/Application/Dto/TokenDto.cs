using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class TokenDto
    {
        public string JWT { get; set; }
        public string RefreshToken { get; set; }
    }
}
