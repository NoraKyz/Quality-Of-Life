using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Quality.Core.Logger
{
    public static class MyLogger
    {
        private static StringBuilder s_stringBuilder = new();

        public static void Init(bool isEnabled)
        {
            Debug.unityLogger.logEnabled = isEnabled;
        }

        public static void Log(string message)
        {
            if (Debug.unityLogger.logEnabled)
            {
                Debug.Log(message);
            }
        }

        public static void Log(this object obj, string message)
        {
            if (Debug.unityLogger.logEnabled)
            {
                Debug.Log(FormatMessage(obj, message));
            }
        }

        public static void LogWarning(string message)
        {
            if (Debug.unityLogger.logEnabled)
            {
                Debug.LogWarning(message);
            }
        }

        public static void LogWarning(this object obj, string message)
        {
            if (Debug.unityLogger.logEnabled)
            {
                Debug.LogWarning(FormatMessage(obj, message));
            }
        }

        public static void LogError(string message)
        {
            if (Debug.unityLogger.logEnabled)
            {
                Debug.LogError(message);
            }
        }

        public static void LogError(this object obj, string message)
        {
            if (Debug.unityLogger.logEnabled)
            {
                Debug.LogError(FormatMessage(obj, message));
            }
        }

        public static string DictionaryToString<TKey, TValue>(Dictionary<TKey, TValue> dict)
        {
            if (dict == null)
            {
                return "null";
            }

            if (dict.Count == 0)
            {
                return "{}";
            }

            s_stringBuilder.Clear();

            foreach (var kvp in dict)
            {
                s_stringBuilder.Append("  ");
                s_stringBuilder.Append(kvp.Key?.ToString() ?? "null");
                s_stringBuilder.Append(": ");
                s_stringBuilder.AppendLine(kvp.Value?.ToString() ?? "null");
            }

            return s_stringBuilder.ToString();
        }

        private static string FormatMessage(object obj, string message)
        {
            s_stringBuilder.Clear();

            s_stringBuilder.Append("[");
            s_stringBuilder.Append(obj.GetType().Name);
            s_stringBuilder.Append("] ");
            s_stringBuilder.Append(message);

            return s_stringBuilder.ToString();
        }
    }
}
