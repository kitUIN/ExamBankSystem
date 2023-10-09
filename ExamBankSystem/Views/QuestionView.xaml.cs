using ExamBankSystem.Args;
using ExamBankSystem.Enums;
using ExamBankSystem.Models;
using ExamBankSystem.ViewModels;
using System;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;

namespace ExamBankSystem.Views
{

    public sealed partial class QuestionView : Page
    {
        private QuestionsViewModel ViewModel { get; } = new QuestionsViewModel();
        public QuestionView()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.ActionEvent += ViewModel_ActionEvent;
            CommandTip.RefreshEvent += CommmandTip_RefreshEvent;
            QuestionMulTip.RefreshEvent += CommmandTip_RefreshEvent;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.ActionEvent -= ViewModel_ActionEvent;
            CommandTip.RefreshEvent -= CommmandTip_RefreshEvent;
            QuestionMulTip.RefreshEvent -= CommmandTip_RefreshEvent;
        }

        private void CommmandTip_RefreshEvent(object sender, EventArgs e)
        {
            ViewModel.Refresh();
        }

        private void ViewModel_ActionEvent(object sender, ActionEventArg e)
        {
            switch (e.TipMode)
            {
                case TipMode.Show:
                    if(e.ActionMode == ActionMode.AddMul)
                    {
                        QuestionMulTip.Show();
                    }
                    else
                    {
                        CommandTip.Show(e.ActionMode, e.Source);
                    }
                    break;
                case TipMode.Hide:
                    CommandTip.Hide();
                    QuestionMulTip.Hide();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 列表选择变化响应
        /// </summary>
        private void MainList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectionChanged(MainList.SelectedItems.Count);
        }

        private void SearchBar3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SearchText = SearchBar3.SelectedIndex.ToString();
            ViewModel.SearchBar_OnTextChanged(SearchBar1, null);
        }

        private void SearchBar2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchBar1.Text = (SearchBar2.SelectedItem as ComboBoxItem).Tag.ToString();
            ViewModel.SearchBar_OnTextChanged(SearchBar1, null);
        }

        private void SearchBar1_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.SearchBar_OnTextChanged(SearchBar1, null);
        }
        private void SearchType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox searcher)
            {
                if (searcher.SelectedIndex == 0)
                {
                    ViewModel.SearchCol = "subjectId";
                    ViewModel.SearchText = "";
                    ViewModel.Search1 = true;
                    ViewModel.Search2 = ViewModel.Search3 = false;
                    ViewModel.SearchBar_OnTextChanged(SearchBar1, null);
                }
                else if (searcher.SelectedIndex == 1)
                {
                    ViewModel.SearchCol = "rank";
                    ViewModel.Search2 = true;
                    ViewModel.SearchBar_OnTextChanged(SearchBar1, null);
                    ViewModel.Search1 = ViewModel.Search3 = false;
                }
                else if (searcher.SelectedIndex == 2)
                {
                    ViewModel.SearchCol = "type";
                    ViewModel.Search3 = true;
                    ViewModel.SearchBar_OnTextChanged(SearchBar1, null);
                    ViewModel.Search1 = ViewModel.Search2 = false;
                }
                else if (searcher.SelectedIndex == 3)
                {
                    ViewModel.SearchCol = "knowledgeId";
                    ViewModel.SearchText = "";
                    ViewModel.Search1 = true;
                    ViewModel.Search2 = ViewModel.Search3 = false;
                    ViewModel.SearchBar_OnTextChanged(SearchBar1, null);
                }
            }
        }
    }
}
