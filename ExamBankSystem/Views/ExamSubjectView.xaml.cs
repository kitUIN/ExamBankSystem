using ExamBankSystem.Args;
using ExamBankSystem.Enums;
using ExamBankSystem.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace ExamBankSystem.Views
{
    /// <summary>
    /// 考试科目页
    /// </summary>
    public sealed partial class ExamSubjectView : Page
    {
        private ExamSubjectViewModel ViewModel { get; } = new ExamSubjectViewModel();

        public ExamSubjectView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.ActionEvent += ViewModel_ActionEvent;
            CommandTip.RefreshEvent += CommandTip_RefreshEvent;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.ActionEvent -= ViewModel_ActionEvent;
            CommandTip.RefreshEvent -= CommandTip_RefreshEvent;
        }

        private void CommandTip_RefreshEvent(object sender, EventArgs e)
        {
            ViewModel.Refresh();
        }

        private void ViewModel_ActionEvent(object sender, ActionEventArg e)
        {
            switch (e.TipMode)
            {
                case TipMode.Show:
                    CommandTip.Show(e.ActionMode, e.Source);
                    break;
                case TipMode.Hide:
                    CommandTip.Hide();
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
    }
}