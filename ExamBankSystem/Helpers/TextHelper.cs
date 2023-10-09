using ExamBankSystem.Extensions;
using ExamBankSystem.Models;
using ExamBankSystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExamBankSystem.Helpers
{
    public class TextHelper
    {
        public static int[,] rankList = new int[5, 5] {
            { 60, 20, 10, 10, 0 },
            { 50, 20, 20, 5, 5 },
            { 40, 20, 20, 15, 5 },
            { 30, 20, 20, 20, 10 },
            { 20, 20, 20, 20, 20 }
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
        /// <summary>
        /// 生成卷子(通过难度
        /// </summary>
        public static async void GeneratePaper(string name, string knowledge, int rank,
            List<int> ints)
        {
            int total = ints.Sum();
            int[] counts = new int[5]
            {
                total * rankList[rank-1,0] / 100,
                total * rankList[rank-1,1] / 100,
                total * rankList[rank-1,2] / 100,
                total * rankList[rank-1,3] / 100,
                total * rankList[rank-1,4] / 100,
            };
            var knowledgePoints = new List<KnowledgePoint>();
            if (string.IsNullOrEmpty(knowledge))
            {
                foreach(var item in knowledge.Split(","))
                {
                    if (await DbHelper.GetKnowledgePointAsync(item) is KnowledgePoint point)
                    {
                        knowledgePoints.Add(point);
                    }
                }
            }
            var questions = new List<List<Question>>()
            {
                new List<Question>(),new List<Question>(),new List<Question>(),
                new List<Question>(),new List<Question>()
            };
            foreach (var item in knowledgePoints)
            {
                for(int i = 0; i < ints.Count; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        questions[j].AddRange(DbHelper.GetQuestionsToTest(i, item.Id, j + 1, counts[j]));
                    }
                }
            }
            for (int i = 0; i < ints.Count; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (questions[j].Count > counts[j])
                    {
                        questions[j] = questions[j].GetRandomChilds( counts[j]);
                    }
                    else if (questions[j].Count < counts[j])
                    {
                        questions[j].AddRange(DbHelper.GetQuestionsToTest(i, j + 1, counts[j] - questions[j].Count));
                        questions[j].RemoveRepeat();
                    }
                }
            }
            double points = 0;
            var res = new List<List<Question>>();
            for(int i = 0; i < ints.Count; i++)
            {
                res.Add(new List<Question>());
            }
            for (int j = 0; j < 5; j++)
            {
                foreach(var item in questions[j])
                {
                    res[(int)item.Type].Add(item);
                    points += item.Point;
                }
            }

            var useId = CurrentData.CurrentUser.Id;
            // 生成
            DbHelper.InsertTestPaper(name, points, false, useId);
            var paper = DbHelper.GetTestPapersByName(name);
            int order = 1;
            for (int j = 0; j < 5; j++)
            {
                foreach (var item in questions[j])
                {
                    DbHelper.InsertQuestionPaper(paper.Id, order++, item.Id, useId);
                }
            }
        }
    }
}
