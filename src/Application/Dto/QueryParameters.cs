namespace Application.Dto
{
    public class QueryParameters
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
        /// <summary>
        /// String for filtering
        /// </summary>
        public string SearchQuery { get; set; }
        /// <summary>
        /// Property to be filtered
        /// </summary>
        public string SearchField { get; set; }
        /// <summary>
        /// Property to be ordered by
        /// </summary>
        public string OrderByField { get; set; }
        /// <summary>
        /// Ascending or Descending sort
        /// </summary>
        public bool OrderByAscending { get; set; } = true;
    }
}
