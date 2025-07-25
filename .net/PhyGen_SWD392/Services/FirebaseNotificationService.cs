using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace PhyGen_SWD392.Services
{
    public class FirebaseNotificationService
    {
        public FirebaseNotificationService(IConfiguration configuration, IWebHostEnvironment env)
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                GoogleCredential credential;

                if (env.IsDevelopment()) 
                {
                    var path = configuration["Firebase:CredentialPath"];
                    credential = GoogleCredential.FromFile(path);
                }
                else 
                {
                    var firebaseJson = configuration["Firebase:CredentialJson"];
                    if (string.IsNullOrWhiteSpace(firebaseJson))
                    {
                        throw new Exception("Firebase__CredentialJson is missing in environment variables.");
                    }
                    credential = GoogleCredential.FromJson(firebaseJson);
                }

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