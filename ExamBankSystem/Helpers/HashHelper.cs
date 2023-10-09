using System;
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
            var res = "";
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(word));
            for (int i = 0; i < s.Length; i++)
            {
                res = res + s[i].ToString(toUpper ? "X2" : "x2");
            }
            return res; 
        }
    }
}