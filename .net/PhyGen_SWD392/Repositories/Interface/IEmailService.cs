namespace PhyGen_SWD392.Repositories.Interface
{
    public interface IEmailService
    {
        Task SendAsync(string toEmail, string subject, string body);
    }
}
