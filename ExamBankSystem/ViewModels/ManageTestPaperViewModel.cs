using ExamBankSystem.Models;
using System.Threading.Tasks;
using Windows.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExamBankSystem.Helpers;

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
        
        [ObservableProperty]
        private string wordA;
        [ObservableProperty]
        private string wordB;
        private StorageFile wordAFile;
        private StorageFile wordBFile;
        [RelayCommand]
        public async Task SelectWordA()
        {
            wordAFile = await FileHelper.GetDocxAsync();
            if(wordAFile != null)
            {
                WordA = wordAFile.DisplayName;
            }
        }
        [RelayCommand]
        public async Task SelectWordB()
        {
            wordBFile = await FileHelper.GetDocxAsync();
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