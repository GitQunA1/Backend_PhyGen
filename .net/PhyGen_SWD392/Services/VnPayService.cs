using Microsoft.Extensions.Options;
using PhyGen_SWD392.Settings;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PhyGen_SWD392.Services
{
    public class VnPayService
    {
        private readonly VnPaySettings _settings;

        public VnPayService(IOptions<VnPaySettings> settings)
        {
            _settings = settings.Value;
        }

        public string CreatePaymentUrl(string orderId, decimal amount, string ipAddress)
        {
            var time = DateTime.UtcNow.AddHours(7);
            var tick = time.ToString("yyyyMMddHHmmss");

            // Step 1: Khởi tạo các tham số vnp
            var vnpParams = new SortedDictionary<string, string>
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", _settings.TmnCode },
                { "vnp_Amount", ((int)(amount * 100)).ToString() }, // nhân 100
                { "vnp_CreateDate", tick },
                { "vnp_CurrCode", "VND" },
                { "vnp_IpAddr", ipAddress },
                { "vnp_Locale", "vn" },
                { "vnp_OrderInfo", $"Thanh toan don hang {orderId}" },
                { "vnp_OrderType", "other" },
                { "vnp_ReturnUrl", _settings.ReturnUrl },
                { "vnp_TxnRef", orderId }
            };

            // Step 2: Tạo chuỗi dữ liệu ký (KHÔNG encode, KHÔNG có vnp_SecureHash)
            var signData = string.Join("&", vnpParams.Select(x => $"{x.Key}={x.Value}"));
            var vnp_SecureHash = HmacSHA512(_settings.HashSecret, signData);

            // Step 3: Thêm chữ ký vào params
            vnpParams.Add("vnp_SecureHash", vnp_SecureHash);

            // Step 4: Tạo URL thanh toán (có encode giá trị)
            var paymentUrl = _settings.Url + "?" +
                string.Join("&", vnpParams.Select(x =>
                    $"{x.Key}={HttpUtility.UrlEncode(x.Value)}"));

            return paymentUrl;
        }

        private string HmacSHA512(string key, string inputData)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var inputBytes = Encoding.UTF8.GetBytes(inputData);
            using var hmac = new HMACSHA512(keyBytes);
            var hash = hmac.ComputeHash(inputBytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}