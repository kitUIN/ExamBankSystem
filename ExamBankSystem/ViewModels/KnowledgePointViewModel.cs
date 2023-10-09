using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExamBankSystem.Args;
using ExamBankSystem.Helpers;
using ExamBankSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using ExamBankSystem.Enums;

namespace ExamBankSystem.ViewModels
{
    public partial class KnowledgePointViewModel : DataTableBase<KnowledgePoint>
    {
        public KnowledgePointViewModel(string searchCol="name") : base(searchCol)
        {
        }
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