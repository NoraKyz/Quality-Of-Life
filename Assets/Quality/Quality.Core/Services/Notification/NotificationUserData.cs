using Quality.Core.SaveLoadData;
using UnityEngine;

namespace Quality.Core.Notification
{
    public class NotificationUserData : UserDataBase
    {
        [SerializeField] private bool isNotification = true;
        
        public override string Key => UserDataKey.NOTIFICATION;
        public bool IsNotification => isNotification;
        
        public void SetNotification(bool isNotification, bool isSave = true)
        {
            this.isNotification = isNotification;
            
            if (isSave)
            {
                DataManager.Save<NotificationUserData>();
            }
        }
    }
}
