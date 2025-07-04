using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace PhyGen_SWD392.Services
{
    public class ImageService
    {
        private readonly Cloudinary _cloudinary;

        public ImageService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "avatars"
            };

            var result = await _cloudinary.UploadAsync(uploadParams);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return result.SecureUrl.ToString();
            }

            throw new Exception("Cloudinary upload failed.");
        }
    }
}