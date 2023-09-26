

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
        /// �����¼�
        /// </summary>
        public event EventHandler<ActionEventArg> ActionEvent;
        [ObservableProperty]
        private bool canEdit = false;
        [ObservableProperty]
        private bool canDelete = false;
        /// <summary>
        /// �����б�
        /// </summary>
        public ObservableCollection<Question> Questions { get; set; } = new ObservableCollection<Question>();
        /// <summary>
        /// �б�ѡ��仯��Ӧ
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
        private void AddOne()
        {
            ActionEvent?.Invoke(this, new ActionEventArg()
            {
                TipMode = TipMode.Show,
                ActionMode = ActionMode.Add,
            });
        }
        [RelayCommand]
        private void Edit(object obj)
        {
            if (obj is ExamSubject subject)
            {
                ActionEvent?.Invoke(this, new ActionEventArg()
                {
                    TipMode = TipMode.Show,
                    ActionMode = ActionMode.Edit,
                    Source = subject.Name
                });
            }
        }
        [RelayCommand]
        private void Delete(object obj)
        {
            ActionEvent?.Invoke(this, new ActionEventArg()
            {
                TipMode = TipMode.Show,
                ActionMode = ActionMode.Delete,
                Source = obj
            });
        }
    }
}
