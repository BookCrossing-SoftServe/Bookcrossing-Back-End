using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Application.Dto.QueryParams
{
    public class PageableParams
    {
        /// <summary>
        /// Page number
        /// </summary>
        [BindRequired]
        public int Page { get; set; }
        /// <summary>
        /// Amount of items displayed per page
        /// </summary>
        [BindRequired]
        public int PageSize { get; set; }
        /// <summary>
        /// Slight optimization, returns amount of items when true
        /// </summary>
        public bool FirstRequest { get; set; } = true;
    }
}
