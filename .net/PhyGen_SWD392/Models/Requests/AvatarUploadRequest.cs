using System.ComponentModel.DataAnnotations;

namespace PhyGen_SWD392.Models.Requests
{
    public class AvatarUploadRequest
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
