namespace PhyGen_SWD392.Repositories.Interface
{
    public interface IEmailVerificationRepo
    {
        Task<bool> RequestEmailVerificationAsync(string email);
        Task<bool> VerifyEmailAsync(string email, string code);
    }
}
