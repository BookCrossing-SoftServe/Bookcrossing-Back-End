using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Application.Dto
{
    public class PaginationDto<T> where T : class
    {
        public List<T> Page { get; set; }
        public int TotalPages { get; set; }
    }
}
