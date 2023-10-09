using CommunityToolkit.Mvvm.ComponentModel;
using ExamBankSystem.Helpers;
using ExamBankSystem.Models;
using ExamBankSystem.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ExamBankSystem.ViewModels
{
    public partial class FindTestPaperViewModel: ObservableObject
    {
        public ObservableCollection<QuestionPaper> RightItems { get; } = new ObservableCollection<QuestionPaper>();
        public ObservableCollection<TestPaper> LeftItems { get; } = new ObservableCollection<TestPaper>();
        public ObservableCollection<KnowledgeDict> Knowledges { get; } = new ObservableCollection<KnowledgeDict>();

        public FindTestPaperViewModel()
        {
            RefreshLeft();
        }
        public void RefreshLeft()
        {
            LeftItems.Clear();
            foreach (var item in DbHelper.GetTestPapersByUser(CurrentData.CurrentUser.Id))
            {
                LeftItems.Add(item);
            }
        }
        /// <summary>
        /// 点击试卷
        /// </summary>
        public void LeftClick(TestPaper paper)
        {
            Debug.WriteLine(paper.Id);
            RightItems.Clear();
            int[] counts = new int[5];
            double[] points = new double[5];
            Dictionary<string, int> knows = new Dictionary<string, int>();
            foreach (var item in DbHelper.GetQuestionsPapersByTestPaper(paper.Id))
            {
                RightItems.Add(item);
                counts[item.Question.Rank - 1] ++;
                points[item.Question.Rank - 1] +=   item.Question.Point;
                if (knows.ContainsKey(item.Question.Knowledge))
                {

                    knows[item.Question.Knowledge]++;
                }
                else
                {

                    knows.Add(item.Question.Knowledge, 1);
                }
            }
            Knowledges.Clear();
            foreach (KeyValuePair<string, int> kv in knows)
            {
                Knowledges.Add(new KnowledgeDict(kv));
            }
            CountRank1 = counts[0].ToString();
            CountRank2 = counts[1].ToString();
            CountRank3 = counts[2].ToString();
            CountRank4 = counts[3].ToString();
            CountRank5 = counts[4].ToString();
            PointRank1 = points[0].ToString();
            PointRank2 = points[1].ToString();
            PointRank3 = points[2].ToString();
            PointRank4 = points[3].ToString();
            PointRank5 = points[4].ToString();
        }

        [ObservableProperty]
        private string countRank1;
        [ObservableProperty]
        private string countRank2;
        [ObservableProperty]
        private string countRank3;
        [ObservableProperty]
        private string countRank4;
        [ObservableProperty]
        private string countRank5;
        [ObservableProperty]
        private string pointRank1;
        [ObservableProperty]
        private string pointRank2;
        [ObservableProperty]
        private string pointRank3;
        [ObservableProperty]
        private string pointRank4;
        [ObservableProperty]
        private string pointRank5;
    }
}
