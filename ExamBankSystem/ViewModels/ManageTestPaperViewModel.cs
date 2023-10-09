using ExamBankSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExamBankSystem.Helpers;
using Microsoft.UI.Xaml.Controls;

namespace ExamBankSystem.ViewModels
{
    public partial class ManageTestPaperViewModel : DataTableBase<TestPaper>
    {
        public ManageTestPaperViewModel(string searchCol = "") : base(searchCol)
        {
        }

        /// <summary>
        /// 是否可以审核
        /// </summary>
        [ObservableProperty] private bool canExamine;

        public void SelectionChanged(TestPaper paper)
        {
            if (paper == null)
            {
                CanDelete = CanEdit = CanExamine = false;
            }
            else
            {
                CanEdit = CanExamine = !paper.IsExamine; 
                CanDelete = true;
            }
        }

        [RelayCommand]
        public void Examine(object obj)
        {
            if (obj is TestPaper paper)
            {
                DbHelper.UpdateTestPaper(paper.Id, "isExamine", true);
                Refresh();
                CanExamine = false;
            }
        }

        [RelayCommand]
        public void ExportPaper(object obj)
        {
        }

        [RelayCommand]
        public void ExportAnswer(object obj)
        {
        }
    }
}