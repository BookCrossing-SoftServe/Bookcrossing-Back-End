using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Application.Dto.QueryParams
{
    public class PageableParams
    {
        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// Amount of items displayed per page
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Slight optimization, returns amount of items when true
        /// </summary>
        public bool FirstRequest { get; set; } = true;
    }
}
