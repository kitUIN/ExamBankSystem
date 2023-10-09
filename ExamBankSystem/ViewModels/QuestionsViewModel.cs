

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExamBankSystem.Args;
using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using ExamBankSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ExamBankSystem.ViewModels
{
    public partial class QuestionsViewModel : DataTableBase<Question>
    {
        
        /// <summary>
        /// 操作事件
        /// </summary>
        public event EventHandler<ActionEventArg> ActionEvent;

        
        /// <summary>
        /// 
        /// </summary>
        public void SelectionChanged(int count)
        {
            CanDelete = CanEdit = count == 1;
        }
    }
}
