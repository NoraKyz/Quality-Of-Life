
namespace Quality.Core.Vibration
{
    public enum VibrationType
    {
        SELECTION,    // case 0 // IOS 10+
        SUCCESS,      // case 1 // IOS 10+
        WARNING,      // case 2 // IOS 10+
        FAILURE,      // case 3 // IOS 10+
        LIGHT_IMPACT,  // case 4 // IOS 10+
        MEDIUM_IMPACT, // case 5 // IOS 10+
        HEAVY_IMPACT,  // case 6 // IOS 10+
        RIGID_IMPACT,  // case 7 // IOS 13+
        SOFT_IMPACT,   // case 8 // IOS 13+
    }
}

