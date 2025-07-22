namespace PhyGen_SWD392.Models.Requests
{
    public class AccountQueryParameters
    {
        public string? Search { get; set; }
        public string? Status { get; set; }
        public string? Role { get; set; }
        public string? SortBy { get; set; }
        public bool Desc { get; set; } = false;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
