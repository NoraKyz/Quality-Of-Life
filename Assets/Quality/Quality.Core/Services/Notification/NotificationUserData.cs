using System;
using Quality.Core.SaveLoadData;
using UnityEngine;

namespace Quality.Core.Notification
{
    [Serializable]
    internal class NotificationUserData : UserDataBase
    {
        [SerializeField] private bool _isNotification = true;

        public override string Key => UserDataKey.NOTIFICATION;
        public bool IsNotification => _isNotification;
        
        public void SetNotification(bool isNotification)
        {
            _isNotification = isNotification;
        }
    }
}
