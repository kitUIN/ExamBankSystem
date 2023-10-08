using ExamBankSystem.Args;
using ExamBankSystem.Enums;
using ExamBankSystem.ViewModels;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


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
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.ActionEvent -= ViewModel_ActionEvent;
            CommandTip.RefreshEvent -= CommmandTip_RefreshEvent;
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
