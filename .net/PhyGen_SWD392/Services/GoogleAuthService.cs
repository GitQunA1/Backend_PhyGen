using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using PhyGen_SWD392.Models.Entities;
using PhyGen_SWD392.Repositories.Interface;

namespace PhyGen_SWD392.Services
{
    public class GoogleAuthService
    {
        private readonly IAuthRepo _authRepo;
        private readonly AuthService _authService;

        private static bool _firebaseInitialized = false;
        private static readonly object _lock = new();

        public GoogleAuthService(IAuthRepo authRepo, AuthService authService)
        {
            _authRepo = authRepo;
            _authService = authService;

            InitializeFirebase();
        }

        private void InitializeFirebase()
        {
            if (!_firebaseInitialized)
            {
                lock (_lock)
                {
                    if (!_firebaseInitialized && FirebaseApp.DefaultInstance == null)
                    {
                        FirebaseApp.Create(new AppOptions
                        {
                            Credential = GoogleCredential.FromFile("firebase-service-account.json")
                        });
                        _firebaseInitialized = true;
                    }
                }
            }
        }

        public async Task<(string? token, Account? account)> LoginWithGoogleAsync(string idToken)
        {
            FirebaseToken decodedToken;

            try
            {
                decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
            }
            catch
            {
                return (null, null); // Token không hợp lệ
            }

            var email = decodedToken.Claims["email"]?.ToString();
            var emailVerified = bool.TryParse(decodedToken.Claims["email_verified"]?.ToString(), out var ev) && ev;

            if (string.IsNullOrWhiteSpace(email))
                return (null, null);

            var account = await _authRepo.GetAccountByEmailAsync(email);

            if (account == null)
            {
                account = new Account
                {
                    Email = email,
                    Username = email.Split('@')[0],
                    Password = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()),
                    EmailVerified = emailVerified,
                    RoleEnum = Models.Enums.RoleEnum.user,
                    AccountTypeEnum = Models.Enums.AccountTypeEnum.free,
                    StatusEnum = Models.Enums.AccountStatusEnum.active,
                 };

                await _authRepo.AddAsync(account);
            }
            var jwt = _authService.GenerateAccessToken(account);
            return (jwt, account);
        }
    }
}
