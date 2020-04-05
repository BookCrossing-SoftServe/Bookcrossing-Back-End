using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class QueryParameters
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public bool FirstRequest { get; set; }
        public string SearchQuery { get; set; }
    }
}
