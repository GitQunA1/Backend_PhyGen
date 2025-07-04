using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace PhyGen_SWD392.Services
{
    public class FirebaseNotificationService
    {
        public FirebaseNotificationService()
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("firebase-service-account.json")
                });
            }
        }

        public async Task<string> SendNotificationAsync(string token, string title, string body)
        {
            var message = new Message()
            {
                Token = token,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                }
            };

            return await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    }
}
