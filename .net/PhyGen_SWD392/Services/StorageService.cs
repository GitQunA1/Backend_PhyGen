using Supabase;

namespace PhyGen_SWD392.Services
{
    public class StorageService
    {
        private readonly Client _supabaseClient;
        private const string BUCKET_NAME = "phygen.examjson";

        public StorageService(IConfiguration configuration)
        {
            var url = configuration["Supabase:Url"];
            var key = configuration["Supabase:Key"];
            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };
            _supabaseClient = new Client(url, key, options);
        }

        public async Task<string> UploadExamJson(string examJson, string fileName)
        {
            try
            {
                // Convert JSON string to byte array
                var bytes = System.Text.Encoding.UTF8.GetBytes(examJson);
                var stream = new MemoryStream(bytes);

                // Upload to Supabase Storage
                await _supabaseClient.Storage
                    .From(BUCKET_NAME)
                    .Upload(bytes, fileName);

                // Get public URL
                var url = _supabaseClient.Storage
                    .From(BUCKET_NAME)
                    .GetPublicUrl(fileName);

                return url;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to upload exam JSON: {ex.Message}");
            }
        }
    }
}
