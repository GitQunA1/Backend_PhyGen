namespace PhyGen_SWD392.Models.Requests
{
    public class ExamMatrixQueryParameters
    {
        public string? Search { get; set; }
        public int? SubjectId { get; set; }
        public string? Status { get; set; }
        public string? ExamType { get; set; }
        public string? SortBy { get; set; }
        public bool Desc { get; set; } = false;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
