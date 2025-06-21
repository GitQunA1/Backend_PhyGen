using PhyGen_SWD392.Models.DTOs;

namespace PhyGen_SWD392.Repositories.Interface
{
    public interface IForgetPasswordRepo
    {
        Task<bool> RequestPasswordResetAsync(string email);

        Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
    }
}