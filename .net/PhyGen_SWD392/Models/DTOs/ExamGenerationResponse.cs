namespace PhyGen_SWD392.Models.DTOs
{
    public class ExamGenerationResponse
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string ExamType { get; set; }
        public List<ExamSectionResponse> Sections { get; set; }
        public string? Error { get; set; }
    }

    public class ExamSectionResponse
    {
        public string SectionName { get; set; }
        public int? DisplayOrder { get; set; }
        public List<ExamQuestionResponse> Questions { get; set; }
    }

    public class ExamQuestionResponse
    {
        public string Content { get; set; }
        public int OrderIndex { get; set; }
        public string DifficultyLevel { get; set; }
        public string Type { get; set; }
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public List<AnswerInfo> Answers { get; set; }
    }
}
