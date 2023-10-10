using ExamBankSystem.Models;
using System.Threading.Tasks;
using Windows.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExamBankSystem.Helpers;
using ExamBankSystem.Enums;
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
            if(obj is TestPaper paper)
            {
                WordHelper.ExportPaper(paper.Id); 
            }
        }

        [RelayCommand]
        public void ExportAnswer(object obj)
        {
            if (obj is TestPaper paper)
            {
                WordHelper.ExportAnswer(paper.Id);
                
            }
        }
        
        [ObservableProperty]
        private string wordA;
        [ObservableProperty]
        private string wordB;
        private StorageFile wordAFile;
        private StorageFile wordBFile;
        [RelayCommand]
        public async Task SelectWordA()
        {
            wordAFile = await FileHelper.GetSingleDocxAsync();
            if(wordAFile != null)
            {
                WordA = wordAFile.DisplayName;
            }
        }
        [RelayCommand]
        public async Task SelectWordB()
        {
            wordBFile = await FileHelper.GetSingleDocxAsync();
            if (wordBFile != null)
            {
                WordB = wordBFile.DisplayName;
            }
        }
        [RelayCommand]
        public void CheckWord()
        {
            wordBFile = null;
            wordAFile = null;
        }
    }
}