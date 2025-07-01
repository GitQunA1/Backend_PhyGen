using Newtonsoft.Json;
using PhyGen_SWD392.Models.Requests;
using System.Net.Http.Headers;
using System.Text;

namespace PhyGen_SWD392.Services
{
    public class FileService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public FileService(IHttpClientFactory factory, IConfiguration config)
        {
            _httpClient = factory.CreateClient();
            _config = config;
        }

        public async Task<AvatarUploadResult> UploadAvatarAsync(IFormFile file, string userId)
        {
            var bucket = "avatars";
            var filePath = $"user-{userId}/{Guid.NewGuid()}-{file.FileName}";
            var supabaseUrl = _config["Supabase:Url"];
            var serviceKey = _config["Supabase:ServiceRoleKey"];

            // Upload
            var uploadUrl = $"{supabaseUrl}/storage/v1/object/{bucket}/{filePath}";
            using var content = new StreamContent(file.OpenReadStream());
            content.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            var uploadRequest = new HttpRequestMessage(HttpMethod.Post, uploadUrl)
            {
                Content = content
            };
            uploadRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", serviceKey);

            var uploadRes = await _httpClient.SendAsync(uploadRequest);
            if (!uploadRes.IsSuccessStatusCode)
                throw new Exception("Upload failed: " + await uploadRes.Content.ReadAsStringAsync());

            // Signed URL
            var signedUrlEndpoint = $"{supabaseUrl}/storage/v1/object/sign/{bucket}/{filePath}";
            var signedReq = new HttpRequestMessage(HttpMethod.Post, signedUrlEndpoint);
            signedReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", serviceKey);
            signedReq.Content = new StringContent(JsonConvert.SerializeObject(new { expiresIn = 600 }),
                Encoding.UTF8, "application/json");

            var signedRes = await _httpClient.SendAsync(signedReq);
            if (!signedRes.IsSuccessStatusCode)
                throw new Exception("Signed URL failed: " + await signedRes.Content.ReadAsStringAsync());

            var signedJson = JsonConvert.DeserializeObject<dynamic>(await signedRes.Content.ReadAsStringAsync());
            return new AvatarUploadResult
            {
                Path = filePath,
                SignedUrl = signedJson.signedURL
            };
        }
    }
}
