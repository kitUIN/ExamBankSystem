using System;
using System.Linq;
using System.Security.Cryptography;

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
        public static string Hash_MD5_32(string word, bool toUpper = true)
        {
            try
            {
                var md5Csp  = new MD5CryptoServiceProvider();

                var bytValue = System.Text.Encoding.UTF8.GetBytes(word);
                var bytHash = md5Csp.ComputeHash(bytValue);
                md5Csp.Clear();
                var sHash = "";
                for (var counter = 0; counter < bytHash.Count(); counter++)
                {
                    long i = bytHash[counter] / 16;
                    var sTemp = "";
                    sTemp = i > 9 ? ((char)(i - 10 + 0x41)).ToString() : ((char)(i + 0x30)).ToString();
                    i = bytHash[counter] % 16;
                    if (i > 9)
                    {
                        sTemp += ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp += ((char)(i + 0x30)).ToString();
                    }
                    sHash += sTemp;
                }
                //根据大小写规则决定返回的字符串
                return toUpper ? sHash : sHash.ToLower();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}