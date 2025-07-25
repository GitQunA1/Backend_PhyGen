using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Services;
using PhyGen_SWD392.ViewModels;
using System.Security.Claims;

namespace PhyGen_SWD392.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly VnPayService _vnPayService;
        private readonly AccountUserService _accountUserService;
        private readonly RedisLogService _redisLogService;

        public PaymentController(VnPayService vnPayService, AccountUserService accountUserService, RedisLogService redisLogService)
        {
            _vnPayService = vnPayService;
            _accountUserService = accountUserService;
            _redisLogService = redisLogService;
        }

        [Authorize]
        [HttpPost("vnpay")]
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

            model.OrderInfo = $"UpgradePremiumForUser:{userId}";

            var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, model);
            return Ok(new { paymentUrl });
        }
        [AllowAnonymous]
        [HttpGet("vnpay/return")]
        public async Task<IActionResult> PaymentCallback()
        {
            var result = _vnPayService.PaymentExecute(Request.Query);

            var responseCode = Request.Query["vnp_ResponseCode"].ToString();
            var transactionStatus = Request.Query["vnp_TransactionStatus"].ToString();

            var isPaidSuccessfully =
                result.Success &&
                responseCode == "00" &&
                transactionStatus == "00";

            var orderInfo = result.OrderDescription;
            int userId = 0;

            if (!string.IsNullOrEmpty(orderInfo) && orderInfo.StartsWith("UpgradePremiumForUser:"))
            {
                int.TryParse(orderInfo.Replace("UpgradePremiumForUser:", ""), out userId);
            }

            await _redisLogService.LogVnPayTransactionAsync(new
            {
                Timestamp = DateTime.UtcNow,
                IsSuccess = isPaidSuccessfully,
                ResponseCode = responseCode,
                TransactionStatus = transactionStatus,
                OrderId = result.OrderId,
                TransactionId = result.TransactionId,
                OrderInfo = orderInfo,
                UserId = userId > 0 ? userId : (int?)null,
                Amount = 599000,
                RawQuery = Request.Query.ToString(),
            });

            if (!isPaidSuccessfully)
            {
                return BadRequest(new
                {
                    status = false,
                    message = "Payment failed, cancelled, or not completed.",
                    responseCode,
                    transactionStatus
                });
            }

            if (userId <= 0)
            {
                return BadRequest(new
                {
                    status = false,
                    message = "Cannot identify user from vnp_OrderInfo."
                });
            }

            var success = await _accountUserService.UpgradeToPremiumAsync(userId);
            if (!success)
            {
                return StatusCode(500, new
                {
                    status = false,
                    message = "Payment succeeded, but failed to upgrade the user account."
                });
            }

            return Ok(new
            {
                status = true,
                message = "Payment successful and account upgraded to Premium!",
                orderId = result.OrderId,
                transactionId = result.TransactionId,
                vnp_ResponseCode = responseCode
            });
        }


    }
}
