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
        public AIGenerationService(HttpClient httpClient, IMapper mapper, PhyGenDbContext context, IExamMatrixRepo examMatrixRepo, IQuestionRepo questionRepo)
        {
            _httpClient = httpClient;
            _mapper = mapper;
            _context = context;
            _examMatrixRepo = examMatrixRepo;
            _questionRepo = questionRepo;
        }

        public async Task<List<AIExamResponse>> GenerateExamFromMatrix(int matrixId, int userId)
        {
            var aiRequest = await BuildAIExamRequest(matrixId);

            var aiResponse = await CallGeminiApi(aiRequest);

            Console.WriteLine("👉 AI Raw Output:");
            Console.WriteLine(aiResponse);
            var selectedQuestions = JsonConvert.DeserializeObject<List<AIExamResponse>>(aiResponse);

            var exam = new Exam
            {
                CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified),
                CreatedBy = userId,
                IsDraft = true,
                Status = "active"
            };
            _context.Exams.Add(exam);
            await _context.SaveChangesAsync();

            var grouped = selectedQuestions.GroupBy(x => x.Section);
            int sectionIndex = 0;

            foreach (var group in grouped)
            {
                var section = new ExamSection
                {
                    ExamId = exam.Id,
                    SectionName = group.Key,
                    DisplayOrder = sectionIndex++
                };
                _context.ExamSections.Add(section);
                await _context.SaveChangesAsync();

                int qIndex = 0;
                foreach (var item in group)
                {
                    _context.ExamQuestions.Add(new ExamQuestion
                    {
                        SectionId = section.Id,
                        QuestionId = item.QuestionId,
                        OrderIndex = qIndex++
                    });
                }
            }

            await _context.SaveChangesAsync();
            return selectedQuestions;
        }

        private async Task<string> CallGeminiApi(AIExamRequest request)
        {
            var apiKey = "AIzaSyC1cvaJ97vriebLpk6qpo7tZoktxdvWCkA";
            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}";

            var prompt = @"
Bạn là một hệ thống tạo đề thi.

Dữ liệu: 
" + JsonConvert.SerializeObject(request) + @"

❗ Yêu cầu:
- Chỉ trả về một mảng JSON hợp lệ.
- Không được bao markdown, không có ```json, không có mô tả.
- TRẢ VỀ DUY NHẤT:

[
  { ""section"": ""Berii"", ""questionId"": 2 }
]

⚠️ KHÔNG TRẢ VỀ:
```json
[
  ...
]
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

        private async Task<AIExamRequest> BuildAIExamRequest(int matrixId)
        {
            var matrix = await _examMatrixRepo.GetByIdAsync(matrixId);
            var matrixDetails = matrix.MatrixSections
                .SelectMany(ms => ms.MatrixDetails)
                .ToList();

            var questions = await _questionRepo.GetQuestionsForMatrixAsync(matrixDetails);

            var matrixDetailDtos = _mapper.Map<List<MatrixDetailDTO>>(matrixDetails);
            var questionDtos = _mapper.Map<List<QuestionDto>>(questions);

            return new AIExamRequest
            {
                MatrixDetails = matrixDetailDtos,
                Questions = questionDtos
            };
        }
    }
}
