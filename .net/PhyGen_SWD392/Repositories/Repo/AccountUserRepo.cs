using Microsoft.EntityFrameworkCore;
using PhyGen_SWD392.Data;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Repositories.Interface;
using BCrypt.Net;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Services;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using PhyGen_SWD392.Models.Requests;
using PhyGen_SWD392.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace PhyGen_SWD392.Repositories.Repo
{
    public class AccountUserRepo : IAccountUser
    {
        private readonly PhyGenDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly SupabaseSettings _settings;

        public AccountUserRepo(PhyGenDbContext context,IHttpClientFactory factory, IConfiguration config, IOptions<SupabaseSettings> options)
        {
            _context = context;
            _httpClient = factory.CreateClient();
            _config = config;
            _settings = options.Value;

        }

        public async Task<Account?> GetByIdAsync(int id)
        {
            return await _context.Accounts.AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> UpdateProfileAsync(int id, UserUpdateDto dto)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) return false;

            account.Username = dto.Username;
            account.Email = dto.Email;
            account.School = dto.School;
            account.Address = dto.Address;   

            if (!string.IsNullOrEmpty(dto.Password))
            {
                account.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }

            _context.Accounts.Update(account);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
        public async Task<List<ExamListDto>> GetExamsByUserAsync(int userId)
        {
            var examList = await (from e in _context.Exams
                                  join em in _context.ExamMatrices on e.Id equals em.Id
                                  join s in _context.Subjects on em.SubjectId equals s.Id
                                  where e.CreatedBy == userId
                                  select new ExamListDto
                                  {
                                      Id = e.Id,
                                      IsDraft = e.IsDraft,
                                      ExamType = e.Examtype,
                                      CreatedAt = e.CreatedAt,
                                      QuestionCount = _context.ExamQuestions
                                          .Count(eq => eq.Section != null && eq.Section.ExamId == e.Id),
                                      SubjectName = s.Name
                                  }).ToListAsync();

            return examList;
        }

        public async Task<AvatarUploadResult> UploadAvatarAsyncSupabase(IFormFile file, string userId)
        {
            var bucket = "user";
            var filePath = $"user-{userId}/{Guid.NewGuid()}-{file.FileName}";
            var supabaseUrl = _config["Supabase:Url"]?.TrimEnd('/');
            var serviceKey = _config["Supabase:ServiceRoleKey"];

            if (string.IsNullOrEmpty(supabaseUrl) || string.IsNullOrEmpty(serviceKey))
                throw new InvalidOperationException("Supabase configuration is missing.");

            // 1. Upload ảnh
            var uploadUrl = $"{supabaseUrl}/storage/v1/object/{bucket}/{filePath}";

            using var content = new StreamContent(file.OpenReadStream());
            content.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            var uploadRequest = new HttpRequestMessage(HttpMethod.Post, uploadUrl)
            {
                Content = content
            };
            uploadRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", serviceKey);

            var uploadResponse = await _httpClient.SendAsync(uploadRequest);
            if (!uploadResponse.IsSuccessStatusCode)
            {
                var error = await uploadResponse.Content.ReadAsStringAsync();
                throw new Exception($"Upload failed: {error}");
            }

            // 2. Tạo Signed URL
            var signUrl = $"{supabaseUrl}/storage/v1/object/sign/{bucket}/{filePath}";

            var signPayload = new
            {
                expiresIn = 600 // 10 phút
            };

            var signRequest = new HttpRequestMessage(HttpMethod.Post, signUrl)
            {
                Content = new StringContent(JsonConvert.SerializeObject(signPayload), Encoding.UTF8, "application/json")
            };
            signRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", serviceKey);

            var signResponse = await _httpClient.SendAsync(signRequest);
            if (!signResponse.IsSuccessStatusCode)
            {
                var error = await signResponse.Content.ReadAsStringAsync();
                throw new Exception($"Signed URL failed: {error}");
            }

            var json = await signResponse.Content.ReadAsStringAsync();
            var signedData = JObject.Parse(json);
            var signedUrl = signedData["signedURL"]?.ToString();

            // 3. Cập nhật AvatarUrl vào DB
            var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null)
                throw new Exception("User not found");

            user.AvatarUrl = filePath;
            await _context.SaveChangesAsync();

            return new AvatarUploadResult
            {
                Path = filePath,
                SignedUrl = signedUrl
            };
        }
    }
}