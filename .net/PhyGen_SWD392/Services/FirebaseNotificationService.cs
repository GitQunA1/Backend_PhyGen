using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace PhyGen_SWD392.Services
{
    public class FirebaseNotificationService
    {
        public FirebaseNotificationService(IConfiguration configuration)
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                var firebaseJson = configuration["Firebase:CredentialJson"];
                var credential = GoogleCredential.FromJson(firebaseJson);

                FirebaseApp.Create(new AppOptions
                {
                    Credential = credential
                });
            }
        }

        public async Task<string> SendNotificationAsync(string deviceToken, string title, string body)
        {
            var message = new Message
            {
                Token = deviceToken,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                }
            };

            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return response; // return messageId
        }
    }
}