namespace PhyGen_SWD392.Models.Entities
{
    public class EmailVerifyInfo
    {
        public string Code { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }
}