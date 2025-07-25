namespace PhyGen_SWD392.Models.Requests
{
    public class MatrixSectionQueryParameters
    {
        public int? MatrixId { get; set; }
        public string? SectionName { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public bool Desc { get; set; } = false;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
