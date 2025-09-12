using System;
using System.Collections;
using Quality.Core.Logger;
using Quality.Core.SaveLoadData;
using Quality.Core.ServiceLocator;
using Unity.Notifications;
using UnityEngine;

namespace Quality.Core.Notification
{
    public class NotificationService : ServiceBase
    {
        [SerializeField] private NofiticationDataSO _notificationDataSO;

        private bool _isAvailable;
        private NotificationUserData _notificationUserData;

        public bool IsNotification => _notificationUserData.IsNotification;
        private NotificationUserData NotificationData => _notificationUserData ??= DataManager.Get<NotificationUserData>();

        public void Initialize()
        {
            var args = NotificationCenterArgs.Default;
            args.AndroidChannelId          = "default";
            args.AndroidChannelName        = "Notifications";
            args.AndroidChannelDescription = "Main notifications";
            NotificationCenter.Initialize(args);

            this.Log("nitialized");

            StartCoroutine(RequestPermission());
        }

        private IEnumerator RequestPermission()
        {
            var request = NotificationCenter.RequestPermission();

            if (request.Status == NotificationsPermissionStatus.RequestPending)
            {
                yield return request;
            }

            _isAvailable = request.Status == NotificationsPermissionStatus.Granted;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if ((_isAvailable && IsNotification) == false)
            {
                return;
            }

            if (hasFocus)
            {
                this.Log($"CancelAllNotifications");
                CancelAllNotifications();
            }
            else
            {
                this.Log($"ScheduleAllNotifications");
                ScheduleAllNotifications();
            }
        }

        public void ScheduleAllNotifications()
        {
            CancelAllNotifications();

            foreach (var data in _notificationDataSO.Notifications)
            {
                SendNotification(data);
            }
        }

        private void CancelAllNotifications()
        {
            NotificationCenter.CancelAllScheduledNotifications();
            NotificationCenter.CancelAllDeliveredNotifications();
        }

        private void SendNotification(NotificationData data)
        {
            var notification = new Unity.Notifications.Notification
            {
                Title       = data.Title,
                Text        = data.Message,
            };

            var fireTime = DateTime.Now.AddHours(data.FireAfterHours);

            NotificationCenter.ScheduleNotification(notification, new NotificationDateTimeSchedule(fireTime));
        }
    }
}
