using PhyGen_SWD392.Models.Enums;

namespace PhyGen_SWD392.Models.DTOs
{
    public class UserUpdateDto
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? School { get; set; }
        public string? Address { get; set; }
        public string Password { get; set; } = null!;
    }
}
