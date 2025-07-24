using PhyGen_SWD392.Models.Requests;

namespace PhyGen_SWD392.Models.DTOs
{
    public class AIExamRequestResult
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public AIExamRequest? Data { get; set; }

        public static AIExamRequestResult Success(AIExamRequest data)
        {
            return new AIExamRequestResult
            {
                IsSuccess = true,
                Data = data,
                ErrorMessage = null
            };
        }

        public static AIExamRequestResult Failure(string errorMessage)
        {
            return new AIExamRequestResult
            {
                IsSuccess = false,
                Data = null,
                ErrorMessage = errorMessage
            };
        }
    }
}
