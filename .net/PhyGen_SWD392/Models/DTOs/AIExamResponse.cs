namespace PhyGen_SWD392.Models.DTOs
{
    public class AIExamResponse
    {
        public string SectionName { get; set; }
        public List<QuestionInfo> Questions { get; set; }
    }
    public class QuestionInfo
    {
        public string QuestionName { get; set; }
        public int OrderIndex { get; set; }
    }
    public class TempAIExamItem
    {
        public string section { get; set; }
        public int questionId { get; set; }
    }
}
