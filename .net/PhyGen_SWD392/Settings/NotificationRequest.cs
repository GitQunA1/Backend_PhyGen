namespace PhyGen_SWD392.Settings
{
    public class NotificationRequest
    {
        public string DeviceToken { get; set; }  // token bên mobile gửi về
        public string Title { get; set; }
        public string Body { get; set; }
        public Dictionary<string, string>? Data { get; set; }
    }
}
