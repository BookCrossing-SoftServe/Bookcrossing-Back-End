using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Application.Dto.QueryParams
{
    public class FullPaginationQueryParams : PageableParams
    {
        public FilterParameters[] Filters { get; set; }
        public SortableParams Sort { get; set; }
    }
}
