namespace PhyGen_SWD392.Models.Requests
{
    public class MatrixDetailQueryParameters
    {
        public int? MatrixSectionId { get; set; }
        public int? TopicId { get; set; }
        public string? DifficultyLevel { get; set; }
        public int? Quantity { get; set; }
        public string? Type { get; set; }
        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public bool Desc { get; set; } = false;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
