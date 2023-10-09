using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ExamBankSystem.Extensions
{
    public static class ValidationExtension
    {
        /// <summary>
        /// 验证密码是否合规,3-16个字符,数字或字母
        /// </summary>
        public static bool IsNotPassword(this string text)
        {
            var pattern = @"^[a-zA-Z0-9]{3,16}$";
            var match = Regex.Match(text, pattern);
            return !match.Success;
        }
        /// <summary>
        /// 验证用户名是否合规,3-16个字符,数字或字母
        /// </summary>
        public static bool IsNotUserName(this string text)
        {
            var pattern = @"^[a-zA-Z0-9]{3,20}$";
            var match = Regex.Match(text, pattern);
            return !match.Success;
        }
    }
}