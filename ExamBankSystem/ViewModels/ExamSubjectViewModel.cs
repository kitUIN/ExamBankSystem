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
using System.Xml.Linq;
using Windows.UI.Xaml.Controls;

namespace ExamBankSystem.ViewModels
{
    public partial class ExamSubjectViewModel: ObservableObject
    {
        /// <summary>
        /// 操作事件
        /// </summary>
        public event EventHandler<ActionEventArg> ActionEvent;
        /// <summary>
        /// 是否允许按下修改按钮
        /// </summary>
        [ObservableProperty]
        private bool canEdit = false;
        /// <summary>
        /// 是否允许按下删除按钮
        /// </summary>
        [ObservableProperty]
        private bool canDelete = false;
        /// <summary>
        /// 考试科目列表
        /// </summary>
        public ObservableCollection<ExamSubject> ExamSubjects { get; set; } = new ObservableCollection<ExamSubject>();
        public ExamSubjectViewModel()
        {
            Refresh();
        }
        /// <summary>
        /// 刷新列表
        /// </summary>
        public void Refresh()
        {
            ExamSubjects.Clear();
            foreach (var item in DbHelper.GetExamSubjects())
            {
                ExamSubjects.Add(item);
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
        private void Edit(object obj)
        {
            if(obj is ExamSubject subject)
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
        private void Add()
        {
            ActionEvent?.Invoke(this, new ActionEventArg()
            {
                TipMode = TipMode.Show,
                ActionMode = ActionMode.Add,
            });
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
