// using Core.Modules.Architecture.SaveLoadData;
// using Newtonsoft.Json;
// using R3;
//
// namespace Core.Modules.Services.Notification
// {
//     internal class NotificationUserData : UserDataBase
//     {
//         public              bool                   isNotification;
//         [JsonIgnore] public ReactiveProperty<bool> isNotificationPresent;
//
//         public NotificationUserData()
//         {
//             isNotification        = true;
//             isNotificationPresent = new ReactiveProperty<bool>(isNotification);
//         }
//
//         public override void InitPresentData()
//         {
//             isNotificationPresent = new ReactiveProperty<bool>(isNotification);
//         }
//
//         public void SetNotification(bool isNotification)
//         {
//             this.isNotification         = isNotification;
//             isNotificationPresent.Value = isNotification;
//             DataManager.Save<NotificationUserData>();
//         }
//     }
// }
