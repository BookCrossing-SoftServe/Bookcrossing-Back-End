using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Dashboard
{
    public class BookUserDataDto
    {
        public Dictionary<DateTime, int> BooksRegistered { get; set; }
        public Dictionary<DateTime, int> UsersRegistered { get; set; }
    }
}
