namespace PhyGen_SWD392.Models.Requests
{
    public class SubjectQueryParameters
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Grade { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public bool Desc { get; set; } = false;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
