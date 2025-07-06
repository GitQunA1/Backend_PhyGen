using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Services;
using PhyGen_SWD392.ViewModels;

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
        [HttpPost("create")]
        public IActionResult CreatePaymentUrl([FromBody] VnPaymentRequestModel model)
        {
            if (model == null || model.Amount <= 0)
                return BadRequest("Thông tin không hợp lệ");

            model.CreatedDate = DateTime.Now;
            var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, model);
            return Ok(new { paymentUrl });
        }

        // Xử lý callback sau khi thanh toán
        [HttpGet("vnpay-return")]
        public IActionResult PaymentCallback()
        {
            // Debug: Hiển thị toàn bộ query string từ VNPAY
            foreach (var key in Request.Query.Keys)
            {
                var value = Request.Query[key];
                Console.WriteLine($"{key}: {value}");
            }

            // Thực hiện xử lý thanh toán
            var result = _vnPayService.PaymentExecute(Request.Query);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = "❌ Thanh toán thất bại hoặc sai chữ ký.",
                    responseCode = result.VnPayResponseCode
                });
            }

            return Ok(new
            {
                message = "✅ Thanh toán thành công!",
                orderId = result.OrderId,
                transactionId = result.TransactionId,
                vnp_ResponseCode = result.VnPayResponseCode,
                token = result.Token
            });
        }
    }
}
