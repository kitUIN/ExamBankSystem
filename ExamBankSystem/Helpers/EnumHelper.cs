using System;
using System.Reflection;

namespace ExamBankSystem.Helpers
{
    /// <summary>
    /// Enum帮助类
    /// </summary>
    public static class EnumHelper
    {
        public static TEnum GetEnum<TEnum>(string text) where TEnum : struct
        {
            if (!typeof(TEnum).GetTypeInfo().IsEnum)
            {
                throw new InvalidOperationException("Generic parameter 'TEnum' must be an enum.");
            }
            return (TEnum)Enum.Parse(typeof(TEnum), text);
        }
    }
}
