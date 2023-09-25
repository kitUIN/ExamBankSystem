using Windows.ApplicationModel.Resources;
using ExamBankSystem.Enums;

namespace ExamBankSystem.Helpers
{
    /// <summary>
    /// 本地化帮助类
    /// </summary>
    public static class ResourcesHelper
    {
        private static readonly ResourceLoader _resourceLoader = ResourceLoader.GetForCurrentView();
        public static string GetString(string key)
        {
            return _resourceLoader.GetString(key);
        }
        /// <summary>
        /// 获取本地化值
        /// </summary>
        public static string GetString(ResourceKey key)
        {
            return GetString(key.ToString());
        }
    }
}