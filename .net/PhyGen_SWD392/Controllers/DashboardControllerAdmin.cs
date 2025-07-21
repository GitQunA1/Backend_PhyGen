using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Services;

namespace PhyGen_SWD392.Controllers
{
    [Route("api/admin/dashboard")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class DashboardControllerAdmin : Controller
    {
        private readonly DashboardService _dashboardService;
        private readonly RedisLogService _redisLogService;

        public DashboardControllerAdmin(DashboardService dashboardService, RedisLogService redisLogService)
        {
            _dashboardService = dashboardService;
            _redisLogService = redisLogService;
        }

        [HttpGet("user/overview")]
        public async Task<IActionResult> GetUserOverview()
        {
            var result = await _dashboardService.GetUserOverviewAsync();
            return Ok(result);
        }

        [HttpGet("monthly/revenue")]
        public async Task<IActionResult> GetMonthlyRevenueFromRedis()
        {
            var data = await _redisLogService.GetRevenueFromRedisAsync();
            return Ok(data);
        }
    }
}
