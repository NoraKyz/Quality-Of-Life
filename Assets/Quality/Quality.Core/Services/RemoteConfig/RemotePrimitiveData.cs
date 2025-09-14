using Newtonsoft.Json;
using UnityEngine;

namespace Quality.Core.RemoteConfig
{
    [JsonObject(MemberSerialization.OptIn)]
    public class RemotePrimitiveData : ScriptableObject
    {
        // define your remote config data structure here
    }
}
