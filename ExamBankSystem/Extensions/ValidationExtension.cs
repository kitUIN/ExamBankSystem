using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ExamBankSystem.Extensions
{
    public static class ValidationExtension
    {
        /// <summary>
        /// 验证是否密码是否合规,6-16个字符必须包含一个字符和一个字母
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsNotPassword(this string text)
        {
            var pattern = @"^(?=.*[a-zA-Z])(?=.*\d).{6,16}$";
            var match = Regex.Match(text, pattern);
            return !match.Success;
        }
    }
}