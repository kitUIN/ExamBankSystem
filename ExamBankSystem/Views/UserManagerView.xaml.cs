using ExamBankSystem.ViewModels;
using System;
using Microsoft.UI.Xaml.Controls;
using ExamBankSystem.Args;
using ExamBankSystem.Enums;
using Microsoft.UI.Xaml.Navigation;

namespace ExamBankSystem.Views
{

    public sealed partial class UserManagerView : Page
    {
        private UserManagerViewModel ViewModel { get; } = new UserManagerViewModel();
        public UserManagerView()
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
                    CommandTip.Show(e.ActionMode,e.Source);
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
        private void MainList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectionChanged(MainList.SelectedItems.Count);
        }
    }
}
