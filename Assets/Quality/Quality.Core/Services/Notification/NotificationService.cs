// using System;
// using System.Collections;
// using Core.Modules.Architecture.SaveLoadData;
// using Core.Modules.Architecture.ServiceLocator;
// using Core.Modules.MyUtilities;
// using R3;
// using Unity.Notifications;
// using UnityEngine;
//
// namespace Core.Modules.Services.Notification
// {
//     public class NotificationService : ServiceBase
//     {
//         [SerializeField] private NofiticationDataSO _notificationDataSO;
//
//         private NotificationUserData _notificationUserData;
//         private NotificationUserData NotificationData => _notificationUserData ??= DataManager.Get<NotificationUserData>();
//
//         public bool IsNotification
//         {
//             get => NotificationData.isNotification;
//             set => NotificationData.SetNotification(value);
//         }
//
//         public ReadOnlyReactiveProperty<bool> IsNotificationPresent => NotificationData.isNotificationPresent;
//
//         private bool _isAvailable;
//
//         public void Initialize()
//         {
//             var args = NotificationCenterArgs.Default;
//             args.AndroidChannelId          = "default";
//             args.AndroidChannelName        = "Notifications";
//             args.AndroidChannelDescription = "Main notifications";
//             NotificationCenter.Initialize(args);
//
//             MyLogger.Log($"[{GetType().Name}] Initialized");
//
//             StartCoroutine(RequestPermission());
//         }
//
//         private IEnumerator RequestPermission()
//         {
//             var request = NotificationCenter.RequestPermission();
//
//             if (request.Status == NotificationsPermissionStatus.RequestPending)
//             {
//                 yield return request;
//             }
//
//             _isAvailable = request.Status == NotificationsPermissionStatus.Granted;
//         }
//
//         private void OnApplicationFocus(bool hasFocus)
//         {
//             if ((_isAvailable && IsNotification) == false)
//             {
//                 return;
//             }
//
//             if (hasFocus)
//             {
//                 MyLogger.Log($"[{GetType().Name}] CancelAllNotifications");
//                 CancelAllNotifications();
//             }
//             else
//             {
//                 MyLogger.Log($"[{GetType().Name}] ScheduleAllNotifications");
//                 ScheduleAllNotifications();
//             }
//         }
//
//         public void ScheduleAllNotifications()
//         {
//             CancelAllNotifications();
//
//             foreach (var data in _notificationDataSO.data)
//             {
//                 SendNotification(data);
//             }
//         }
//
//         private void CancelAllNotifications()
//         {
//             NotificationCenter.CancelAllScheduledNotifications();
//             NotificationCenter.CancelAllDeliveredNotifications();
//         }
//
//         private void SendNotification(NotificationData data)
//         {
//             var notification = new Unity.Notifications.Notification
//             {
//                 Title       = data.title,
//                 Text        = data.message,
//             };
//
//             var fireTime = DateTime.Now.AddHours(data.fireAfterHours);
//
//             NotificationCenter.ScheduleNotification(notification, new NotificationDateTimeSchedule(fireTime));
//         }
//     }
// }
