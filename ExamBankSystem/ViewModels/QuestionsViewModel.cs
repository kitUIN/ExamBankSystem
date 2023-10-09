using System;
using System.Collections.Generic;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using ExamBankSystem.Models;
using Microsoft.UI.Xaml.Controls;
using ExamBankSystem.Helpers;

namespace ExamBankSystem.ViewModels
{
    public partial class QuestionsViewModel : DataTableBase<Question>
    {
        public QuestionsViewModel(string searchCol= "subjectId") : base(searchCol)
        {
        }
        [ObservableProperty]
        private bool search1 = true;
        [ObservableProperty]
        private bool search2;
        [ObservableProperty]
        private bool search3;
        /// <summary>
        /// 
        /// </summary>
        public void SelectionChanged(int count)
        {
            CanDelete = CanEdit = count == 1;
        }
        /// <summary>
        /// 搜索刷新
        /// </summary>
        /// <param name="text"></param>
        public override async void SearchRefresh(string text)
        {
            Debug.WriteLine(text);
            Debug.WriteLine(SearchCol);
            List<Question> items = new List<Question>();
            if (SearchCol == "type" || SearchCol == "rank")
            {
                if (text == "0" && SearchCol == "rank") text = "1";
                items = await DbHelper.GetQuestionAsync(SearchCol, Convert.ToInt32(text), CurrentPage);
                TotalPage = (DbHelper.GetQuestionCountAsync(SearchCol, Convert.ToInt32(text)) + 14) / 15;
            }
            else if (SearchCol == "subjectId")
            {
                var subjects = await DbHelper.SearchAsync<ExamSubject>("subject", text);
                foreach (var subject in subjects)
                {
                    items.AddRange(await DbHelper.GetQuestionAsync(SearchCol, subject.Id));
                }
                TotalPage = 1;
            }
            else if (SearchCol == "knowledgeId")
            {
                var subjects = await DbHelper.SearchAsync<KnowledgePoint>("name", text);
                foreach (var subject in subjects)
                {
                    items.AddRange(await DbHelper.GetQuestionAsync(SearchCol, subject.Id));
                }
                TotalPage = 1;
            }
            else
            {
                items = await DbHelper.GetAsync<Question>(CurrentPage);
                TotalPage = (DbHelper.GetCount<Question>() + 14) / 15;
            }
            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
    }
}
