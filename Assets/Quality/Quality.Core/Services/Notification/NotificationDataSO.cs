using System.Collections.Generic;
using UnityEngine;

namespace Core.Modules.Services.Notification
{
    [CreateAssetMenu(fileName = "so-notification-data", menuName = "SO/Services/Notification/Notification Data")]
    public class NofiticationDataSO : ScriptableObject
    {
        [SerializeField] private List<NotificationData> _notifications = new();
        
        public IReadOnlyList<NotificationData> Notifications => _notifications;
    }
}
