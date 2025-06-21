using PhyGen_SWD392.Dtos;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Repositories.Interface;

namespace PhyGen_SWD392.Services
{
    public class AccountUserService
    {
        private readonly IAccountUser _accountUserRepo;

        public AccountUserService(IAccountUser accountUserRepo)
        {
            _accountUserRepo = accountUserRepo;
        }

        public async Task<Account?> GetByIdAsync(int id)
        {
            return await _accountUserRepo.GetByIdAsync(id);
        }

        public async Task<bool> UpdateProfileAsync(int id, UserUpdateDto dto)
        {
            return await _accountUserRepo.UpdateProfileAsync(id, dto);
        }
    }
}