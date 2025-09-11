using System.Collections.Generic;
using UnityEngine;

namespace Core.Modules.Services.Notification
{
    [CreateAssetMenu(fileName = "so-notification-data", menuName = "SO/Services/Notification/Notification Data")]
    public class NofiticationDataSO : ScriptableObject
    {
        public List<NotificationData> data = new List<NotificationData>();
    }
}
