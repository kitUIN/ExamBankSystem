using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExamBankSystem.Args;
using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using ExamBankSystem.Models;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace ExamBankSystem.ViewModels
{
    public partial class ExamSubjectViewModel : DataTableBase<ExamSubject>
    {
        
        /// <summary>
        /// 操作事件
        /// </summary>
        public event EventHandler<ActionEventArg> ActionEvent;

        protected new string SearchCol = "subject";

        
        /// <summary>
        /// 列表选择变化响应
        /// </summary>
        public void SelectionChanged(int count)
        {
            CanEdit = count == 1;
            CanDelete = count > 0;
        }
        
    }
}