using Quality.Core.SaveLoadData;
using Quality.Core.ServiceLocator;
using Solo.MOST_IN_ONE;
using UnityEngine;

namespace Quality.Core.Vibration
{
    public class VibrationService : ServiceBase
    {
        [SerializeField] private VibrationUserData _vibrationUserData;

        private bool IsVibrateable => IsVibration && SystemInfo.supportsVibration && Most_HapticFeedback.IsSupported();
        private VibrationUserData VibrationData => _vibrationUserData ??= DataManager.Get<VibrationUserData>();

        public bool IsVibration
        {
            get => VibrationData.IsVibration;
            set => VibrationData.SetVibration(value);
        }

        public void Vibrate(VibrationType type)
        {
            if (IsVibrateable == false)
            {
                return;
            }

            Most_HapticFeedback.Generate((Most_HapticFeedback.HapticTypes) type);
        }

        public void VibrateWithCooldown(VibrationType type, float cooldown)
        {
            if (IsVibrateable == false)
            {
                return;
            }

            Most_HapticFeedback.GenerateWithCooldown((Most_HapticFeedback.HapticTypes) type);
        }
    }
}

