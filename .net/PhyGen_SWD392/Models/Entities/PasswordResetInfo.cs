namespace PhyGen_SWD392.Models.Entities
{
    public class PasswordResetInfo
    {
        public string Code { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }
}
