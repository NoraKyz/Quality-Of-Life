using System;
using UnityEngine;

namespace Quality.Core.Notification
{
    [Serializable]
    public class NotificationData
    {
        [SerializeField] private string _title;
        [SerializeField] private string _message;
        [SerializeField] private float  _fireAfterHours;
        
        public string Title => _title;
        public string Message => _message;
        public float  FireAfterHours => _fireAfterHours;
    }
}
