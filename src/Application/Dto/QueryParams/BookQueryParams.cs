using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Application.Dto.QueryParams
{
    public class BookQueryParams : PageableParams
    {
        public string SearchTerm { get; set; }
        public int[] Genres { get; set; }
        public int? Location { get; set; }
        public bool? ShowAvailable { get; set; }  

        public SortableParams SortableParams { get; set; }
    }
}
