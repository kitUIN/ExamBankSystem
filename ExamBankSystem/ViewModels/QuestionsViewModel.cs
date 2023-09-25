

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

namespace ExamBankSystem.ViewModels
{
    public partial class QuestionsViewModel : ObservableObject
    {
        /// <summary>
        /// 操作事件
        /// </summary>
        public event EventHandler<ActionEventArg> ActionEvent;
        [ObservableProperty]
        private bool canEdit = false;
        [ObservableProperty]
        private bool canDelete = false;
        /// <summary>
        /// 知识点列表
        /// </summary>
        public ObservableCollection<Question> Questions { get; set; } = new ObservableCollection<Question>();
        /// <summary>
        /// 列表选择变化响应
        /// </summary>
        public void SelectionChanged(int count)
        {
            CanDelete = CanEdit = count == 1;
        }
        public QuestionsViewModel()
        {
            Refresh();
        }
        public void Refresh()
        {
            Questions.Clear();
            foreach (var item in DbHelper.GetQuestions())
            {
                Questions.Add(item);
            }
        }
        [RelayCommand]
        private void Edit()
        {

        }
        [RelayCommand]
        private void AddOne()
        {
            ActionEvent?.Invoke(this, new ActionEventArg()
            {
                TipMode = TipMode.Show,
                ActionMode = ActionMode.Add,
            });
        }
        [RelayCommand]
        private void Delete()
        {

        }
    }
}
