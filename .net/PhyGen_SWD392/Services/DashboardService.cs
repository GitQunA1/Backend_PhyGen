using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Repositories.Interface;

namespace PhyGen_SWD392.Services
{
    public class DashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<UserOverviewDto> GetUserOverviewAsync()
        {
            return await _dashboardRepository.GetUserOverviewAsync();
        }
    }
}