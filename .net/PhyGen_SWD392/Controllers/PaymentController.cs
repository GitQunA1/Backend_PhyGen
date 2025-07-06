using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Services;
using PhyGen_SWD392.ViewModels;
using System.Security.Claims;

namespace PhyGen_SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly VnPayService _vnPayService;
        private readonly AccountUserService _accountUserService;

        public PaymentController(VnPayService vnPayService, AccountUserService accountUserService)
        {
            _vnPayService = vnPayService;
            _accountUserService = accountUserService;
        }

        [Authorize]
        [HttpPost("create")]
        public IActionResult CreatePaymentUrl([FromBody] VnPaymentRequestModel model)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c =>
                c.Type == "id" || c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized("Cannot identify user from token.");

            model.OrderId = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            model.CreatedDate = DateTime.Now;
            model.FullName = "User #" + userId;
            model.Amount = 599000;

            model.Description = $"UpgradePremiumForUser:{userId}";

            var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, model);
            return Ok(new { paymentUrl });
        }

        [Authorize]
        [HttpGet("vnpay-return")]
        public async Task<IActionResult> PaymentCallback()
        {
            foreach (var key in Request.Query.Keys)
            {
                var value = Request.Query[key];
                Console.WriteLine($"{key}: {value}");
            }

            var result = _vnPayService.PaymentExecute(Request.Query);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = "❌ Payment failed or the signature is invalid.",
                    responseCode = result.VnPayResponseCode
                });
            }

            var userIdClaim = User.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier || c.Type == "sub" || c.Type == "id");

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("Unable to identify user from the token.");
            }

            var success = await _accountUserService.UpgradeToPremiumAsync(userId);
            if (!success)
            {
                return StatusCode(500, "Payment succeeded, but failed to upgrade the user account.");
            }

            return Ok(new
            {
                message = "✅ Payment successful and account upgraded to Premium!",
                orderId = result.OrderId,
                transactionId = result.TransactionId,
                vnp_ResponseCode = result.VnPayResponseCode,
                token = result.Token
            });
        }

    }
}
