using PhyGen_SWD392.Repositories.Interface;

namespace PhyGen_SWD392.Services
{
    public class PaymentService
    {
        private readonly IAccountUser _accountRepo;
        private readonly FirebaseNotificationService _firebaseService;

        public PaymentService(IAccountUser accountRepo, FirebaseNotificationService firebaseService)
        {
            _accountRepo = accountRepo;
            _firebaseService = firebaseService;
        }

        public async Task<bool> UpgradeToPremiumAsync(int userId, string deviceToken)
        {
            var success = await _accountRepo.UpdateUserTypeAsync(userId);
            if (!success) return false;

            if (!string.IsNullOrEmpty(deviceToken))
            {
                await _firebaseService.SendNotificationAsync(
                    deviceToken,
                    "Thanh toán thành công",
                    "Tài khoản của bạn đã được nâng cấp lên Premium!"
                );
            }
            return true;
        }
    }
}
