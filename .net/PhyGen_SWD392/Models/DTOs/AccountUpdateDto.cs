using PhyGen_SWD392.Models.Enums;

namespace PhyGen_SWD392.Models.DTOs
{
    public class AccountUpdateDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public bool? EmailVerified { get; set; }

        public RoleEnum? Role { get; set; }
        public AccountTypeEnum? AccountType { get; set; }
        public AccountStatusEnum? AccountStatus { get; set; }
    }
}
