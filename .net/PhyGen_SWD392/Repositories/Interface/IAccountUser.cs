using PhyGen_SWD392.Dtos;
using PhyGen_SWD392.Models.Entities;

namespace PhyGen_SWD392.Repositories.Interface
{
    public interface IAccountUser
    {
        Task<Account?> GetByIdAsync(int id);
        Task<bool> UpdateProfileAsync(int id, UserUpdateDto dto);
<<<<<<< Updated upstream
=======
        Task<List<ExamListDto>> GetExamsByUserAsync(int userId);
        Task<bool> UpdateAvatarUrlAsync(int userId, string avatarUrl);
        Task<bool> UpdateUserTypeAsync(int userId);

>>>>>>> Stashed changes
    }
}
