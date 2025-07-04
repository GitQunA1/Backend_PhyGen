using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Helpers;
using PhyGen_SWD392.Models.DTOs;
using PhyGen_SWD392.Repositories.Interface;
using PhyGen_SWD392.Services;
using PhyGen_SWD392.Settings;
using System.Security.Claims;
using System.Text;

namespace PhyGen_SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly FirebaseNotificationService _firebaseService;
        private readonly PaymentService _paymentService;

        public NotificationController(FirebaseNotificationService firebaseService, PaymentService paymentService)
        {
            _firebaseService = firebaseService;
            _paymentService = paymentService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] SendNotificationDTO dto)
        {
            var result = await _firebaseService.SendNotificationAsync(dto.Token, dto.Title, dto.Body);
            return Ok(new { messageId = result });
        }

        [HttpPost("upgrade-premium")]
        public async Task<IActionResult> UpgradePremium([FromBody] UpgradePremiumDTO dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("id");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(ApiResponse<string>.ErrorResponse(
                    new List<string> { "Unauthorized" },
                    "Unable to identify the user from the token."
                ));
            }

            var success = await _paymentService.UpgradeToPremiumAsync(userId, dto.DeviceToken);
            if (!success)
            {
                return NotFound(ApiResponse<string>.ErrorResponse(
                    new List<string> { "Upgrade failed" },
                    "Account not found or upgrade failed."
                ));
            }

            return Ok(ApiResponse<string>.SuccessResponse(
                "Premium upgrade successful",
                "Account successfully upgraded to Premium and notification sent."
            ));
        }
    }
}
