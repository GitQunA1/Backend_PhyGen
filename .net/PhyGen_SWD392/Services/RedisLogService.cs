using StackExchange.Redis;
using System.Text.Json;

namespace PhyGen_SWD392.Services
{
    public class RedisLogService
    {
        private readonly IDatabase _db;
        private readonly JsonSerializerOptions _jsonOptions;

        public RedisLogService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };
        } 
        public async Task LogVnPayTransactionAsync(object data)
        {
            string key = $"vnpay:logs:{DateTime.UtcNow:yyyyMMdd}";
            string json = JsonSerializer.Serialize(data, _jsonOptions);

            await _db.ListLeftPushAsync(key, json);
        }

        public async Task<List<string>> GetLogsAsync(string? date = null, int limit = 100)
        {
            string key = $"vnpay:logs:{(date ?? DateTime.UtcNow.ToString("yyyyMMdd"))}";

            var entries = await _db.ListRangeAsync(key, 0, limit - 1); 
            return entries.Select(e => e.ToString()).ToList();
        }
        public async Task<Dictionary<string, decimal>> GetRevenueFromRedisAsync()
        {
            var revenueByMonth = new Dictionary<string, decimal>();

            for (int monthOffset = 0; monthOffset < 6; monthOffset++) 
            {
                var date = DateTime.UtcNow.AddMonths(-monthOffset);
                var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);

                decimal monthlyTotal = 0;

                for (int day = 1; day <= daysInMonth; day++)
                {
                    var keyDate = new DateTime(date.Year, date.Month, day);
                    var dayKey = $"vnpay:logs:{keyDate:yyyyMMdd}";
                    var logs = await _db.ListRangeAsync(dayKey, 0, -1);

                    foreach (var entry in logs)
                    {
                        try
                        {
                            using var doc = JsonDocument.Parse(entry.ToString());
                            var root = doc.RootElement;

                            if (root.TryGetProperty("isSuccess", out var isSuccess) && isSuccess.GetBoolean())
                            {
                                if (root.TryGetProperty("amount", out var amountProp) &&
                                    amountProp.TryGetDecimal(out var amount))
                                {
                                    monthlyTotal += amount;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Redis log parse error: {ex.Message}");
                        }
                    }
                }

                var monthKey = $"{date:yyyy-MM}";
                revenueByMonth[monthKey] = monthlyTotal;
            }

            return revenueByMonth;
        }

    }
}
