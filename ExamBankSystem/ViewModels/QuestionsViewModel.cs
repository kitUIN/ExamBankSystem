using System;
using System.Collections.Generic;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using ExamBankSystem.Models;
using Windows.UI.Xaml.Controls;
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
        public new async void SearchRefresh(string text)
        {
            List<Question> items = new List<Question>();
            if (SearchCol == "type" || SearchCol == "rank")
            {
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
            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
        public void SearchType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(sender is ComboBox searcher)
            {
                if (searcher.SelectedIndex == 0)
                {
                    SearchCol = "subjectId";
                    SearchText = "";
                    Search1 = true;
                    Search2 = Search3 = false;
                }
                else if (searcher.SelectedIndex == 1)
                {
                    SearchCol = "rank";
                    SearchText = "0";
                    Search2 = true;
                    Search1 = Search3 = false;
                }
                else if (searcher.SelectedIndex == 2)
                {
                    SearchCol = "type";
                    SearchText = "0";
                    Search3 = true;
                    Search1 = Search2 = false;
                }
                else if (searcher.SelectedIndex == 3)
                {
                    SearchCol = "knowledgeId";
                    SearchText = "";
                    Search1 = true;
                    Search2 = Search3 = false;
                }
            }
        }
    }
}
