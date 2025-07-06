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

        public PaymentController(VnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        [Authorize]
        [HttpPost("create")]
        public IActionResult CreatePaymentUrl([FromBody] VnPaymentRequestModel model)
        {
            if (model == null || model.Amount <= 0)
                return BadRequest("Invalid payment information.");

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? "unknown@example.com";


            model.OrderId = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            model.CreatedDate = DateTime.Now;
            model.FullName = email;
            model.Amount = 599000;

            var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, model);
            return Ok(new { paymentUrl });
        }

        [HttpGet("vnpay-return")]
        public IActionResult PaymentCallback()
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
                    message = "Payment failed or invalid signature.",
                    responseCode = result.VnPayResponseCode
                });
            }

            return Ok(new
            {
                message = "✅ Payment successful!",
                orderId = result.OrderId,
                transactionId = result.TransactionId,
                vnp_ResponseCode = result.VnPayResponseCode,
                token = result.Token
            });
        }
    }
}
