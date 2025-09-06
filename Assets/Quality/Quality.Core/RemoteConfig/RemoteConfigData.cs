using Newtonsoft.Json;
using UnityEngine;

namespace Quality.Core.RemoteConfig
{
    public interface IReadOnlyRemoteConfigData
    {
        public bool IsOpenAds { get; }
        public bool IsNativeAds { get; }
        public bool IsBannerAds { get; }
        public bool IsInterAds { get; }
        public bool IsRewardedAds { get; }

        public int LevelStartAds { get; }

        public bool IsDebugAppsflyer { get; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class RemoteConfigData : ScriptableObject, IReadOnlyRemoteConfigData
    {
        // Ads Config
        [JsonProperty] public bool isOpenAds     = true;
        [JsonProperty] public bool isNativeAds   = true;
        [JsonProperty] public bool isBannerAds   = true;
        [JsonProperty] public bool isInterAds    = true;
        [JsonProperty] public bool isRewardedAds = true;
        [JsonProperty] public int  levelStartAds = 3;

        // Tracking Config
        [JsonProperty] public bool isDebugAppsflyer = true;

        #region IReadOnly Implements

        public bool IsOpenAds => isOpenAds;
        public bool IsNativeAds => isNativeAds;
        public bool IsBannerAds => isBannerAds;
        public bool IsInterAds => isInterAds;
        public bool IsRewardedAds => isRewardedAds;

        public int LevelStartAds => levelStartAds;

        public bool IsDebugAppsflyer => isDebugAppsflyer;

        #endregion
    }
}
