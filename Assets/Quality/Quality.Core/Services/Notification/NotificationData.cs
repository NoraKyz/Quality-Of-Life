using System;

namespace Core.Modules.Services.Notification
{
    [Serializable]
    public class NotificationData
    {
        public string title;
        public string message;
        public float  fireAfterHours;
    }
}
