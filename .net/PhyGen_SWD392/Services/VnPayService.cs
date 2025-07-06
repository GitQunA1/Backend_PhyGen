using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PhyGen_SWD392.Helpers;
using PhyGen_SWD392.Settings;
using PhyGen_SWD392.ViewModels;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PhyGen_SWD392.Services
{
    public class VnPayService
    {
        private readonly IConfiguration _config;
        public VnPayService(IConfiguration config)
        {
            _config = config;
        }
        public string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model)
        {
            var tick = DateTime.Now.Ticks.ToString();

            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", _config["VnPay:Version"]);
            vnpay.AddRequestData("vnp_Command", _config["VnPay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString());

            vnpay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _config["VnPay:Locale"]);

            vnpay.AddRequestData("vnp_OrderInfo", model.OrderInfo);
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", _config["VnPay:PaymentBackReturnUrl"]);
            vnpay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl = vnpay.CreateRequestUrl(_config["VnPay:BaseUrl"], _config["VnPay:HashSecret"]);

            return paymentUrl;
        }

        public VnPaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var vnpay = new VnPayLibrary();

            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            var vnp_SecureHash = collections["vnp_SecureHash"];

            if (string.IsNullOrEmpty(vnp_SecureHash) || !vnpay.ValidateSignature(vnp_SecureHash, _config["VnPay:HashSecret"]))
            {
                return new VnPaymentResponseModel
                {
                    Success = false,
                    VnPayResponseCode = "INVALID_SIGNATURE"
                };
            }

            var txnRefStr = vnpay.GetResponseData("vnp_TxnRef");
            if (!long.TryParse(txnRefStr, out long vnp_orderId))
            {
                return new VnPaymentResponseModel
                {
                    Success = false,
                    VnPayResponseCode = "INVALID_TxnRef"
                };
            }

            var transNoStr = vnpay.GetResponseData("vnp_TransactionNo");
            long.TryParse(transNoStr, out long vnp_TransactionId);

            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

            return new VnPaymentResponseModel
            {
                Success = true,
                PaymentMethod = "VnPay",
                OrderDescription = vnp_OrderInfo,
                OrderId = vnp_orderId.ToString(),
                TransactionId = vnp_TransactionId.ToString(),
                Token = vnp_SecureHash,
                VnPayResponseCode = vnp_ResponseCode
            };
        }
    }
}