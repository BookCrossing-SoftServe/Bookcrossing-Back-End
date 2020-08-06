using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.QueryParams
{
    public class OuterSourceQueryParameters
    {
        public string SearchTerm { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
