using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhyGen_SWD392.Services;

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

        [HttpGet("create")]
        public IActionResult CreatePaymentUrl(string orderId, decimal amount)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
            var url = _vnPayService.CreatePaymentUrl(orderId, amount, ip);
            return Ok(new { paymentUrl = url });
        }

        [HttpGet("vnpay-return")]
        public IActionResult PaymentCallback()
        {
            var query = Request.Query;
            var vnp_ResponseCode = query["vnp_ResponseCode"];
            var vnp_TxnRef = query["vnp_TxnRef"];

            if (vnp_ResponseCode == "00")
            {
                return Ok("Thanh toán thành công! Mã đơn hàng: " + vnp_TxnRef);
            }

            return BadRequest("Thanh toán thất bại");
        }
    }
}