

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
    public partial class QuestionsViewModel : DataTableBase
    {
        /// <summary>
        /// 操作事件
        /// </summary>
        public event EventHandler<ActionEventArg> ActionEvent;

        /// <summary>
        /// 是否允许按下修改按钮
        /// </summary>
        [ObservableProperty] private bool canEdit;

        /// <summary>
        /// 是否允许按下删除按钮
        /// </summary>
        [ObservableProperty] private bool canDelete;
        /// <summary>
        /// 题目列表
        /// </summary>
        public ObservableCollection<Question> Questions { get; } = new ObservableCollection<Question>();
        /// <summary>
        /// 
        /// </summary>
        public void SelectionChanged(int count)
        {
            CanDelete = CanEdit = count == 1;
        }
        

        /// <summary>
        /// 添加按钮响应
        /// </summary> 
        [RelayCommand]
        private void Add()
        {
            ActionEvent?.Invoke(this, new ActionEventArg()
            {
                TipMode = TipMode.Show,
                ActionMode = ActionMode.Add,
            });
        }

        /// <summary>
        /// 修改按钮响应
        /// </summary>
        /// <param name="obj">选中的列</param>
        [RelayCommand]
        private void Edit(object obj)
        {
            ActionEvent?.Invoke(this, new ActionEventArg()
            {
                TipMode = TipMode.Show,
                ActionMode = ActionMode.Edit,
                Source = obj
            });
        }

        /// <summary>
        /// 删除按钮响应
        /// </summary>
        /// <param name="obj">选中的列</param>
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
