using System;
using System.Collections.Generic;
using System.Text;
using Domain.RDBMS.Entities;

namespace Application.Dto
{
    class BookDetailsDto
    {
        public string Name { get; set; }

        public BookState State { get; set; }
    }
}
