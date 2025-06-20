namespace PhyGen_SWD392.Dtos
{
    public class AccountLoginDto
    {
        public string Identifier { get; set; } = null!; // Email hoặc Username
        public string Password { get; set; } = null!;
    }
}
