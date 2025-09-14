using System;
using Quality.Core.SaveLoadData;
using UnityEngine;

namespace Quality.Core.Vibration
{
    [Serializable]
    public class VibrationUserData : UserDataBase
    {
        [SerializeField] private bool _isVibration = true;

        public bool IsVibration => _isVibration;

        public override string Key => UserDataKey.VIBRATION;

        public void SetVibration(bool isVibration)
        {
            _isVibration = isVibration;
        }
    }
}

