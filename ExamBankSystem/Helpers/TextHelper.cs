using System;

namespace ExamBankSystem.Helpers
{
    public class TextHelper
    {
        public static double CheckText(string s1,string s2)
        {
            int n1 = s1.Length;
            int n2 = s2.Length;
            int[,] dp = new int[n1 + 1, n2 + 1];
            for(int i = 1; i <= n1; ++i)
            {
                for(int j = 1; j <= n2; ++j)
                {
                    if (s1[i - 1] == s2[j - 1])
                    {
                        dp[i,j] = dp[i - 1,j - 1] + 1;
                    }
                    else
                    {
                        dp[i, j] = Math.Max(dp[i - 1, j], dp[i, j - 1]);
                    }
                }
            }
            return (double) dp[n1, n2] / Math.Max(n1, n2);
        }
    }
}
