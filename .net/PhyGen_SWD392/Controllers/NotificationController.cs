using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Services;

namespace PhyGen_SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly FirebaseNotificationService _firebaseService;

        public NotificationController(FirebaseNotificationService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        [HttpPost("send-premium-success")]
        public async Task<IActionResult> SendPremiumUpgradeNotification([FromBody] string deviceToken)
        {
            var title = "Nâng cấp thành công";
            var body = "Tài khoản của bạn đã được nâng cấp lên Premium!";

            var messageId = await _firebaseService.SendNotificationAsync(deviceToken, title, body);

            return Ok(new
            {
                message = "✅ Đã gửi thông báo nâng cấp thành công!",
                firebaseMessageId = messageId
            });
        }
    }
}

