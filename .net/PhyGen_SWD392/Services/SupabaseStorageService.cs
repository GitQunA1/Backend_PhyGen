using Supabase;

namespace PhyGen_SWD392.Services
{
    public class SupabaseStorageService
    {
        private readonly Supabase.Client _client;

        public SupabaseStorageService(IConfiguration config)
        {
            var url = config["Supabase:Url"];
            var key = config["Supabase:ServiceRoleKey"]; // cần Service Role Key cho signed URL
            _client = new Supabase.Client(url, key, new SupabaseOptions { AutoConnectRealtime = false });
        }

        public async Task<string> GenerateSignedAvatarUrl(string path)
        {
            await _client.InitializeAsync();
            var bucket = _client.Storage.From("user");
            var signedUrl = await bucket.CreateSignedUrl(path, 3600);
            return signedUrl;
        }
    }
}