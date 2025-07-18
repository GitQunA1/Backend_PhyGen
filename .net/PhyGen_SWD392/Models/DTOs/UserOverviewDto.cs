using PhyGen_SWD392.Models.Enums;

namespace PhyGen_SWD392.Models.DTOs
{
    public class UserOverviewDto
    {
        public int TotalUsers { get; set; }
        public int EmailVerifiedUsers { get; set; }
        public Dictionary<AccountTypeEnum, int> UsersByAccountType { get; set; }
    }
}
