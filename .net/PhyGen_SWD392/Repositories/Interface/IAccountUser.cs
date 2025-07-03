using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Models.Requests;

namespace PhyGen_SWD392.Repositories.Interface
{
    public interface IAccountUser
    {
        Task<Account?> GetByIdAsync(int id);
        Task<bool> UpdateProfileAsync(int id, UserUpdateDto dto);
        Task<List<ExamListDto>> GetExamsByUserAsync(int userId);
        Task<bool> UpdateAvatarUrlAsync(int userId, string avatarUrl);
    }
}
