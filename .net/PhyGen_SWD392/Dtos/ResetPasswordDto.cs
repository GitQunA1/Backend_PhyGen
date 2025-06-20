namespace PhyGen_SWD392.Dtos
{
    public class ResetPasswordDto
    {
        public string Email { get; set; } = null!;
        public string VerificationCode { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
