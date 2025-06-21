
using PhyGen_SWD392.Models.Enums;

namespace PhyGen_SWD392.Dtos
{
    public class AccountUpdateDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool? EmailVerified { get; set; } = false;
        public RoleEnum Role { get; set; }
        public AccountTypeEnum AccountType { get; set; } 
        public AccountStatusEnum AccountStatus { get; set; }
    }
}
