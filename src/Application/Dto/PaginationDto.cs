using System;
using System.Collections.Generic;

namespace Application.Dto
{
    public class PaginationDto<T> where T : class
    {
        public List<T> Page { get; set; }
        public int TotalCount { get; set; }
    }
}
