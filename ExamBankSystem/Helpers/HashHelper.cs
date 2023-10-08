using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ExamBankSystem.Helpers
{
    public static class HashHelper
    {
        /// <summary>
        /// 计算32位MD5码
        /// </summary>
        /// <param name="word">字符串</param>
        /// <param name="toUpper">返回哈希值格式 true：英文大写，false：英文小写</param>
        /// <returns></returns>
        public static string Hash_MD5_32(string word, bool toUpper = false)
        {
            var md5 = new MD5CryptoServiceProvider();
            var sHash = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(word))).Replace("-","");
            return toUpper ? sHash : sHash.ToLower();
        }
    }
}