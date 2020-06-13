using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Dashboard
{
    public class BookUserDataDto
    {
        public Dictionary<string, int> BooksRegistered { get; set; }
        public Dictionary<string, int> UsersRegistered { get; set; }
    }
}
