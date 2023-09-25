using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    public partial class ExamSubjectViewModel: ObservableObject
    {
        [ObservableProperty]
        private bool canEdit = false;
        [ObservableProperty]
        private bool canDelete = false;
        /// <summary>
        /// 考试科目列表
        /// </summary>
        public ObservableCollection<ExamSubject> ExamSubjects { get; set; } = new ObservableCollection<ExamSubject>();
        public ExamSubjectViewModel()
        {
            foreach(var examSubject in DbHelper.GetExamSubject())
            {
                ExamSubjects.Add(examSubject);
            }
        }
        /// <summary>
        /// 列表选择变化响应
        /// </summary>
        public void SelectionChanged(int count)
        {
            CanEdit = count == 1;
            CanDelete = count > 0;
        }
        [RelayCommand]
        private void Edit()
        {

        }
        [RelayCommand]
        private void Add()
        {

        }
        [RelayCommand]
        private void Delete()
        {

        }
    }
}
