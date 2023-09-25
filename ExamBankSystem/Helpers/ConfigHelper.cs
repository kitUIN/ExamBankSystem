using ExamBankSystem.Enums;
using System;
using System.Configuration;
using Windows.Storage;

namespace ExamBankSystem.Helpers
{

    public static class ConfigHelper
    {
        public static void Init()
        {
            if (Contains(ConfigKey.LastUserName))
            {
                Set(ConfigKey.LastUserName, "");
            }
            if (Contains(ConfigKey.LastUserPassword))
            {
                Set(ConfigKey.LastUserPassword, "");
            }
        }
        private const string Container = "ExamBankSystem";

        public static bool Contains(string key, string container = Container)
        {
            var coreSettings =
                    ApplicationData.Current.LocalSettings.CreateContainer(container,
                        ApplicationDataCreateDisposition.Always);
            return coreSettings.Values.ContainsKey(key);
        }
        public static bool Contains(ConfigKey key)
        {
            return Contains(key.ToString());
        }

        private static object Get(string key, string container = Container)
        {
            var coreSettings =
                   ApplicationData.Current.LocalSettings.CreateContainer(container,
                       ApplicationDataCreateDisposition.Always);
            return coreSettings.Values.TryGetValue(key, out var value) ? value : null;
        }

        public static void Set(string key, object value, string container = Container)
        {
            var coreSettings =
                    ApplicationData.Current.LocalSettings.CreateContainer(container,
                        ApplicationDataCreateDisposition.Always);
            coreSettings.Values[key] = value;
        }
        public static void Set(ConfigKey key, object value)
        {
            Set(key.ToString(), value);
        }
        public static string GetString(string key, string container = Container)
        {
            return (string)Get(key, container);
        }
        public static string GetString(ConfigKey key)
        {
            return GetString(key.ToString());
        }

        public static bool GetBoolean(string key, string container = Container)
        {
            var res = Get(key, container);
            if (res == null) return false;
            return (bool)res;
        }

        public static int GetInt32(string key, string container = Container)
        {
            var res = Get(key, container);
            if (res == null) return 0;
            return (int)res;
        }

        public static long GetInt64(string key, string container = Container)
        {
            var res = Get(key, container);
            if (res == null) return 0;
            return (long)res;
        }

        public static double GetDouble(string key, string container = Container)
        {
            var res = Get(key, container);
            if (res == null) return 0;
            return (double)res;
        }

        public static float GetFloat(string key, string container = Container)
        {
            var res = Get(key, container);
            if (res == null) return 0;
            return (float)res;
        }

        public static DateTime GetDateTime(string key, string container = Container)
        {
            var res = Get(key, container);
            if (res == null) return default;
            return (DateTime)res;
        }
    }
}