using System;
using UnityEngine;
using ZLinq;

namespace Quality.Core.Utilities
{
    public enum VersionComparisonResult
    {
        LESSER = -1,
        EQUAL = 0,
        GREATER = 1
    }

    public class SystemUtility
    {
        public static int GetDeviceRam()
        {
            return SystemInfo.systemMemorySize == 0 ? 9999 : SystemInfo.systemMemorySize;
        }

        public static VersionComparisonResult CompareVersions(string versionA, string versionB)
        {
            if (versionA.Equals(versionB)) return VersionComparisonResult.EQUAL;

            // Check if either of the versions are beta versions. Beta versions could be of format x.y.z-beta or x.y.z-betaX.
            // Split the version string into beta component and the underlying version.
            int piece;
            
            var isVersionABeta = versionA.Contains("-beta");
            var versionABetaNumber = 0;
            if (isVersionABeta)
            {
                var components = versionA.Split(new[] {"-beta"}, StringSplitOptions.None);
                versionA = components[0];
                versionABetaNumber = int.TryParse(components[1], out piece) ? piece : 0;
            }

            var isVersionBBeta = versionB.Contains("-beta");
            var versionBBetaNumber = 0;
            if (isVersionBBeta)
            {
                var components = versionB.Split(new[] {"-beta"}, StringSplitOptions.None);
                versionB = components[0];
                versionBBetaNumber = int.TryParse(components[1], out piece) ? piece : 0;
            }

            // Now that we have separated the beta component, check if the underlying versions are the same.
            if (versionA.Equals(versionB))
            {
                // The versions are the same, compare the beta components.
                if (isVersionABeta && isVersionBBeta)
                {
                    if (versionABetaNumber < versionBBetaNumber) return VersionComparisonResult.LESSER;

                    if (versionABetaNumber > versionBBetaNumber) return VersionComparisonResult.GREATER;
                }
                // Only VersionA is beta, so A is older.
                else if (isVersionABeta)
                {
                    return VersionComparisonResult.LESSER;
                }
                // Only VersionB is beta, A is newer.
                else
                {
                    return VersionComparisonResult.GREATER;
                }
            }

            // Compare the non beta component of the version string.
            var versionAComponents = versionA.Split('.').AsValueEnumerable().Select(version => int.TryParse(version, out piece) ? piece : 0).ToArray();
            var versionBComponents = versionB.Split('.').AsValueEnumerable().Select(version => int.TryParse(version, out piece) ? piece : 0).ToArray();
            var length = Mathf.Max(versionAComponents.Length, versionBComponents.Length);
            for (var i = 0; i < length; i++)
            {
                var aComponent = i < versionAComponents.Length ? versionAComponents[i] : 0;
                var bComponent = i < versionBComponents.Length ? versionBComponents[i] : 0;

                if (aComponent < bComponent) return VersionComparisonResult.LESSER;

                if (aComponent > bComponent) return VersionComparisonResult.GREATER;
            }

            return VersionComparisonResult.EQUAL;
        }
    }
}
