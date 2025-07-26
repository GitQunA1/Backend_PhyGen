using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Requests;
using PhyGen_SWD392.Repositories.Interface;
using PhyGen_SWD392.Repositories.Repo;
using System.Text;

namespace PhyGen_SWD392.Services
{
    public class AIGenerationService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly PhyGenDbContext _context;
        private readonly IExamMatrixRepo _examMatrixRepo;
        private readonly IQuestionRepo _questionRepo;
        private readonly StorageService _storageService;
        public AIGenerationService(HttpClient httpClient, IMapper mapper, PhyGenDbContext context, IExamMatrixRepo examMatrixRepo, IQuestionRepo questionRepo, StorageService storageService)
        {
            _httpClient = httpClient;
            _mapper = mapper;
            _context = context;
            _examMatrixRepo = examMatrixRepo;
            _questionRepo = questionRepo;
            _storageService = storageService;
        }

        public async Task<ExamGenerationResponse> GenerateExamFromMatrix(int matrixId, int userId)
        {
            var buildResult = await BuildAIExamRequest(matrixId);
            if (!buildResult.IsSuccess)
            {
                return new ExamGenerationResponse
                {
                    Error = buildResult.ErrorMessage,
                };
            }

            var aiRequest = await BuildAIExamRequest(matrixId);
            var aiResponse = await CallGeminiApi(buildResult.Data);

            Console.WriteLine("👉 AI Raw Output:");
            Console.WriteLine(aiResponse);

            if (aiResponse.StartsWith("```json"))
            {
                aiResponse = aiResponse.Replace("```json", "").Trim();
            }
            if (aiResponse.EndsWith("```"))
            {
                aiResponse = aiResponse.Replace("```", "").Trim();
            }

            var tempItems = JsonConvert.DeserializeObject<List<TempAIExamItem>>(aiResponse);

            var matrix = await _examMatrixRepo.GetByIdAsync(matrixId);

            var exam = new Exam
            {
                CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified),
                CreatedBy = userId,
                IsDraft = true,
                Status = "active",
                Examtype = matrix.Examtype
            };
            _context.Exams.Add(exam);
            await _context.SaveChangesAsync(); 

            var grouped = tempItems.GroupBy(x => x.section);

            var examResponse = new ExamGenerationResponse
            {
                ExamId = exam.Id,
                SubjectId = matrix.SubjectId ?? 0,
                SubjectName = matrix.Subject?.Name,
                ExamType = matrix.Examtype,
                Sections = new List<ExamSectionResponse>()
            };

            foreach (var group in grouped)
            {
                var matrixSection = matrix.MatrixSections.FirstOrDefault(x => x.SectionName == group.Key);
                if (matrixSection == null) continue;

                var totalQuantityRequired = matrixSection.MatrixDetails.Sum(md => md.Quantity ?? 0);
                var selectedQuestions = group.Take(totalQuantityRequired).ToList();

                var section = new ExamSection
                {
                    ExamId = exam.Id,
                    SectionName = group.Key,
                    DisplayOrder = matrixSection?.DisplayOrder ?? 0
                };
                _context.ExamSections.Add(section);
                await _context.SaveChangesAsync();

                var sectionResponse = new ExamSectionResponse
                {
                    SectionName = group.Key,
                    DisplayOrder = matrixSection.DisplayOrder,
                    Questions = new List<ExamQuestionResponse>()
                };

                int qIndex = 1;
                foreach (var item in selectedQuestions)
                {
                    var question = await _questionRepo.GetByIdAsync(item.questionId);
                    if (question == null) continue;

                    _context.ExamQuestions.Add(new ExamQuestion
                    {
                        SectionId = section.Id,
                        QuestionId = item.questionId,
                        OrderIndex = qIndex
                    });

                    var answers = question.Answers
                        .Where(a => a.IsDeleted != true)
                        .Select(a => new AnswerInfo
                        {
                            Content = a.Content,
                            IsCorrect = a.IsCorrect
                        })
                        .ToList();

                    sectionResponse.Questions.Add(new ExamQuestionResponse
                    {
                        Content = question.Content,
                        OrderIndex = qIndex,
                        DifficultyLevel = question.DifficultyLevel,
                        Type = question.Type,
                        TopicId = question.TopicId ?? 0,
                        TopicName = question.Topic?.Name,
                        Answers = answers
                    });

                    qIndex++;
                }
                examResponse.Sections.Add(sectionResponse);
            }

            try
            {
                var examJson = JsonConvert.SerializeObject(examResponse);
                var fileName = $"exam_{exam.Id}_{DateTime.UtcNow:yyyyMMddHHmmss}.json";
            
                var jsonUrl = await _storageService.UploadExamJson(examJson, fileName);
                // Update exam với URL
                exam.ExamJson = jsonUrl;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to upload exam JSON: {ex.Message}");
            }

            return examResponse;
        }

        private async Task<string> CallGeminiApi(AIExamRequest request)
        {
            var apiKey = "AIzaSyC1cvaJ97vriebLpk6qpo7tZoktxdvWCkA";
            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}";

            var prompt = @"
Bạn là một hệ thống hỗ trợ tạo đề thi Vật Lý.
Nhiệm vụ của bạn là chọn các câu hỏi từ danh sách 'questions' để phù hợp với từng phần trong 'matrixDetails' của đề thi.

Dữ liệu đầu vào (đã được serialize từ 'request'):
" + JsonConvert.SerializeObject(request) + @"

Quy tắc chọn câu hỏi:
- Mỗi phần trong 'matrixDetails' có 'sectionName', 'topic', 'type' 'difficultyLevel' và 'quantity'.
- Bạn phải chọn chính xác 'quantity' câu hỏi cho mỗi 'sectionName'.
- Các câu hỏi được chọn phải khớp với 'topic', 'type' và 'difficultyLevel' của phần đó.
- Danh sách câu hỏi để chọn là 'questions'.

ĐỊNH DẠNG ĐẦU RA YÊU CẦU:
Bạn CHỈ được trả về MỘT MẢNG JSON. Mỗi phần tử trong mảng là một đối tượng JSON có hai trường:
- ""section"": TÊN CỦA PHẦN ĐỀ (từ 'sectionName' trong 'matrixDetails').
- ""questionId"": ID CỦA CÂU HỎỎI ĐƯỢC CHỌN (từ 'id' trong 'questions').

VÍ DỤ VỀ ĐỊNH DẠNG ĐẦU RA BẮT BUỘC:
[
  { ""section"": ""Điện học"", ""questionId"": 2 },
  { ""section"": ""Cơ học"", ""questionId"": 4 }
]

NHẮC LẠI:
- KHÔNG BAO GIỜ TRẢ VỀ BẤT KỲ VĂN BẢN GIẢI THÍCH NÀO.
- KHÔNG BAO GIỜ TRẢ VỀ BẤT KỲ MÔ TẢ NÀO.
- KHÔNG BAO GIỜ TRẢ VỀ BẤT KỲ DẤU HIỆU MARKDOWN NÀO (ví dụ: ```json).
- CHỈ DUY NHẤT MẢNG JSON THEO ĐỊNH DẠNG TRÊN.
- ĐẢM BẢO CHỌN ĐÚNG SỐ LƯỢNG VÀ ĐÚNG ĐIỀU KIỆN.
";

            var content = new
            {
                contents = new[]
                {
                new {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                    }
                }
            };

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(httpRequest);
            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine("🧠 Gemini raw response:");
            Console.WriteLine(json);

            dynamic result = JsonConvert.DeserializeObject(json);

            if (result?.candidates == null || result.candidates.Count == 0)
            {
                throw new Exception("Gemini không trả về kết quả hợp lệ.");
            }

            return result.candidates[0]?.content?.parts?[0]?.text ?? throw new Exception("Không tìm thấy text từ Gemini.");
        }

        private async Task<AIExamRequestResult> BuildAIExamRequest(int matrixId)
        {
            var matrix = await _examMatrixRepo.GetByIdAsync(matrixId);
            if (matrix == null || matrix.MatrixSections == null || !matrix.MatrixSections.Any())
            {
                return AIExamRequestResult.Failure("Matrix không có section nào, không thể tạo đề thi.");
            }

            var matrixDetails = matrix.MatrixSections
                .SelectMany(ms => ms.MatrixDetails)
                .ToList();

            var questions = await _questionRepo.GetQuestionsForMatrixDetailAsync(matrixDetails);

            foreach (var detail in matrixDetails)
            {
                var matchingQuestions = questions.Where(q =>
                    q.TopicId == detail.TopicId &&
                    q.Type == detail.Type &&
                    q.DifficultyLevel == detail.DifficultyLevel
                ).ToList();
                if (matchingQuestions.Count < detail.Quantity)
                {
                    return AIExamRequestResult.Failure($"Không đủ câu hỏi cho detail: topic={detail.TopicId}, type={detail.Type}, level={detail.DifficultyLevel}");
                }
            }
            var matrixDetailDtos = _mapper.Map<List<MatrixDetailDTO>>(matrixDetails);
            var questionDtos = _mapper.Map<List<QuestionDto>>(questions);

            var aiRequest = new AIExamRequest
            {
                MatrixDetails = matrixDetailDtos,
                Questions = questionDtos
            };

            return AIExamRequestResult.Success(aiRequest);
        }
    }
}
