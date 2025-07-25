using PhyGen_SWD392.Models.Enums;

namespace PhyGen_SWD392.Models.DTOs
{
    public class AccountRegisterDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
    }
}
