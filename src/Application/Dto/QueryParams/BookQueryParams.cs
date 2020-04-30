using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Application.Dto.QueryParams
{
    public class BookQueryParams : PageableParams
    {
        public string SearchTerm { get; set; }
        public int[] GenreIds { get; set; }
        public int? SelectedLocationId { get; set; }
        public bool? ShowAvailable { get; set; }
    }
}
