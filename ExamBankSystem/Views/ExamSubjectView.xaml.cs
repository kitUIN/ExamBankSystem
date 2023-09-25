using ExamBankSystem.Args;
using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
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
            ExamSubjectTip.RefreshEvent += ExamSubjectTip_RefreshEvent;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.ActionEvent -= ViewModel_ActionEvent;
            ExamSubjectTip.RefreshEvent -= ExamSubjectTip_RefreshEvent;
        }

        private void ExamSubjectTip_RefreshEvent(object sender, EventArgs e)
        {
            ViewModel.Refresh();
        }

        private void ViewModel_ActionEvent(object sender, ActionEventArg e)
        {
            if(e.TipMode == TipMode.Show)
            {
                ExamSubjectTip.Show(e.ActionMode,e.Source);
            }
            else if(e.TipMode == TipMode.Hide)
            {
                ExamSubjectTip.Hide();
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
