using ExamBankSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ExamBankSystem.Args;
using ExamBankSystem.Enums;


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
