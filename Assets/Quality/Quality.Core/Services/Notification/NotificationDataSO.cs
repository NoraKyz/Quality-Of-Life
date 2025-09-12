using System.Collections.Generic;
using UnityEngine;

namespace Quality.Core.Notification
{
    public class NofiticationDataSO : ScriptableObject
    {
        [SerializeField] private List<NotificationData> _notifications = new();
        
        public IReadOnlyList<NotificationData> Notifications => _notifications;
    }
}
