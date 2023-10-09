using ExamBankSystem.ViewModels;
using System;
using ExamBankSystem.Args;
using ExamBankSystem.Enums;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Controls;

namespace ExamBankSystem.Views
{
    public sealed partial class KnowledgePointView : Page
    {
        private KnowledgePointViewModel ViewModel { get; } = new KnowledgePointViewModel();

        public KnowledgePointView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 列表选择变化响应
        /// </summary>
        private void MainList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectionChanged(MainList.SelectedItems.Count);
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
    }
}
