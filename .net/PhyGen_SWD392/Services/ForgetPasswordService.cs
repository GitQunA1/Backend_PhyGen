using PhyGen_SWD392.Dtos;
using PhyGen_SWD392.Repositories.Interface;

namespace PhyGen_SWD392.Services
{
    public class ForgetPasswordService
    {
        private readonly IForgetPasswordRepo _repo;

        public ForgetPasswordService(IForgetPasswordRepo repo)
        {
            _repo = repo;
        }

        public Task<bool> RequestPasswordResetAsync(string email)
        {
            return _repo.RequestPasswordResetAsync(email);
        }

        public Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            return _repo.ResetPasswordAsync(dto);
        }
    }
}