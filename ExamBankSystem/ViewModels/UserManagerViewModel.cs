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
    public partial class UserManagerViewModel : DataTableBase<User>
    {
        /// <summary>
        /// 操作事件
        /// </summary>
        public event EventHandler<ActionEventArg> ActionEvent;

        /// <summary>
        /// 重置按钮是否亮起
        /// </summary>
        [ObservableProperty] private bool canReset;
        
        /// <summary>
        /// 列表选择变化响应
        /// </summary>
        public void SelectionChanged(int count)
        {
            CanDelete = CanReset = count > 0;
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="obj">选中的列</param>
        [RelayCommand]
        private void Reset(object obj)
        {
            ActionEvent?.Invoke(this, new ActionEventArg()
            {
                TipMode = TipMode.Show,
                ActionMode = ActionMode.Reset,
                Source = obj
            });
        }
    }
}