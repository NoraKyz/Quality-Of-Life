using System;
using Newtonsoft.Json;

namespace Quality.Core.SaveLoadData
{
    [Serializable]
    public abstract class UserDataBase
    {
        private string _dataKey;

        [JsonIgnore]
        public string DataKey
        {
            get => _dataKey ??= GetType().Name;
        }

        public virtual void InitPresentData()
        {

        }
    }
}
