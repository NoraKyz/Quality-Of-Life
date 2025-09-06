using System.Collections.Generic;
using System.Reflection;
using ZLinq;

namespace Quality.Core.RemoteConfig
{
    internal static class RemoteConfigHelper
    {
        public static List<string> GetAllConfigKey()
        {
            var remoteConfigType = typeof(RemoteConfigData);
            var fields = remoteConfigType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            return fields.AsValueEnumerable().Select(field => field.Name).ToList();
        }
    }
}
