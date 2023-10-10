using ExamBankSystem.Extensions;
using ExamBankSystem.Models;
using ExamBankSystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Windows.Storage;

namespace ExamBankSystem.Helpers
{
    public class TextHelper
    {
        public static int[] rankList = new int[5] {
            55,35,20,10,5
        };

        /// <summary>
        /// 检查重合度
        /// </summary>
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
        public static void GeneratePaper1(string name, int rank,
            List<int> ints, List<Question> questions)
        {
            var res = new List<List<Question>>();
            for (int i = 0; i < ints.Count; i++)
            {
                res.Add(new List<Question>());
            }
            for (int i = 0; i < ints.Count; ++i)
            {
                res[i].AddRange(DbHelper.GetQuestionsToTest(i, ints[i]));
            }
            for (int i = 0; i < ints.Count; ++i)
            {
                for (int j = 0; j < res[i].Count; ++j)
                {
                    Random rand = new Random();
                    var t = rand.Next(1, 100);
                    if (rankList[Math.Abs(res[i][j].Rank - rank)] > t)
                    {
                        while (true)
                        {
                            res[i][j] = DbHelper.GetQuestionsToTest(i, 1)[0];
                            for (int k = 0; k < j; k++)
                            {
                                if (res[i][k].Id == res[i][j].Id)
                                {
                                    continue;
                                }
                            }
                            break;
                        }
                    }
                }
            }
            // 删除与之前的重复
            foreach (var item in questions)
            {
                for (int i = 0; i < ints.Count; ++i)
                {
                    foreach (var j in res[i])
                    {
                        if (TextHelper.CheckText(j.Name, item.Name) > CurrentData.CheckedPercent)
                        {
                            res[i].Remove(j);
                        }
                    }
                }
            }
            InserPage(name, res);
        }
        public static void GeneratePaper2(string name, int rank,
            List<int> ints, List<Question> questions)
        {

            for (int p = 0; p < 2; p++)
            {
                var total = new int[7];
                bool f = p == 0 ? true : false;
                var res = new List<List<Question>>();
                for (int i = 0; i < ints.Count; i++)
                {
                    res.Add(new List<Question>());
                    total[i] = (int)DbHelper.GetQuestionCountAsync("type", i);
                }
                for (int i = 0; i < ints.Count; ++i)
                {
                    res[i].AddRange(DbHelper.GetQuestionsToTest(i, ints[i], total[i], f));
                }
                for (int i = 0; i < ints.Count; ++i)
                {
                    for (int j = 0; j < res[i].Count; ++j)
                    {
                        Random rand = new Random();
                        var t = rand.Next(1, 100);
                        if (rankList[Math.Abs(res[i][j].Rank - rank)] > t)
                        {
                            while (true)
                            {
                                res[i][j] = DbHelper.GetQuestionsToTest(i, 1, total[i], f)[0];
                                for (int k = 0; k < j; k++)
                                {
                                    if (res[i][k].Id == res[i][j].Id)
                                    {
                                        continue;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
                // 删除与之前的重复
                foreach (var item in questions)
                {
                    for (int i = 0; i < ints.Count; ++i)
                    {
                        foreach (var j in res[i])
                        {
                            if (TextHelper.CheckText(j.Name, item.Name) > CurrentData.CheckedPercent)
                            {
                                res[i].Remove(j);
                            }
                        }
                    }
                }
                InserPage(name + (f?"A":"B"), res);
            }

        }
        /// <summary>
        /// 生成卷子(通过难度
        /// </summary>
        public static void GeneratePaper(string name, string knowledge, int rank,
            List<int> ints, List<string> files, int mode)
        {
            if (mode == 0)
            {
                var res = new List<Question>();
                foreach (var item in files)
                {
                    res.AddRange(WordHelper.ImportPaper(item));
                }
                GeneratePaper1(name, rank, ints, res);
            }
            else
            {
                var res = new List<Question>();
                foreach (var item in files)
                {
                    res.AddRange(WordHelper.ImportPaper(item));
                }
                GeneratePaper2(name, rank, ints, res);
            }
        } 

        public static void InserPage(string name, List<List<Question>> questions)
        {
            double points = 0;
            var useId = CurrentData.CurrentUser.Id;
            DbHelper.InsertTestPaper(name, points, false, useId);
            var paper = DbHelper.GetTestPapersByName(name);
            int order = 1;
            foreach (var items in questions)
            {
                foreach (var item in items)
                {
                    DbHelper.InsertQuestionPaper(paper.Id, order++, item.Id, useId);
                    points += item.Point;
                }
            }
            DbHelper.UpdateTestPaper(paper.Id, "point", points);
        }
    }
}
