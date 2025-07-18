using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Models.Enums;

namespace PhyGen_SWD392.Repositories.Interface
{
    public interface IDashboardRepository
    {
        Task<UserOverviewDto> GetUserOverviewAsync();
    }
}
