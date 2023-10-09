using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExamBankSystem.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;  

namespace ExamBankSystem.ViewModels
{
    public partial class MergeTestPaperViewModel: ObservableObject
    {
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
        public void Merge()
        {
            wordBFile = null;
            wordAFile = null;
        }
    }
}
