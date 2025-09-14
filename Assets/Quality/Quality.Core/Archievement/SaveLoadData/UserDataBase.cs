using Newtonsoft.Json;

namespace Quality.Core.SaveLoadData
{
    [JsonObject(MemberSerialization.Fields)]
    public abstract class UserDataBase
    {
        public abstract string Key { get; }
    }
}
