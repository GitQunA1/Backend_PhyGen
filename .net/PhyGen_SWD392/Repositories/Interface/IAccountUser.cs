using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Entities;

namespace PhyGen_SWD392.Repositories.Interface
{
    public interface IAccountUser
    {
        Task<Account?> GetByIdAsync(int id);
        Task<bool> UpdateProfileAsync(int id, UserUpdateDto dto);
    }
}
