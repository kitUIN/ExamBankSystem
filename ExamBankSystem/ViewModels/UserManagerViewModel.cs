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
    public partial class UserManagerViewModel: ObservableObject
    {
        /// <summary>
        /// 操作事件
        /// </summary>
        public event EventHandler<ActionEventArg> ActionEvent;
        /// <summary>
        /// 重置按钮是否亮起
        /// </summary>
        [ObservableProperty]
        private bool canReset = false;
        /// <summary>
        /// 删除按钮是否亮起
        /// </summary>
        [ObservableProperty]
        private bool canDelete = false;
        /// <summary>
        /// 用户列表
        /// </summary>
        public ObservableCollection<User> Users { get; } = new ObservableCollection<User>();
        public UserManagerViewModel()
        {
            Refresh();
        }
        /// <summary>
        /// 刷新列表
        /// </summary>
        public void Refresh()
        {
            Users.Clear();
            foreach (var item in DbHelper.GetUsers())
            {
                Users.Add(item);
            }
        }
        /// <summary>
        /// 列表选择变化响应
        /// </summary>
        public void SelectionChanged(int count)
        {
            CanDelete = CanReset = count > 0;
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
        private void Reset(object obj)
        {
            ActionEvent?.Invoke(this, new ActionEventArg()
            {
                TipMode = TipMode.Show,
                ActionMode = ActionMode.Reset,
                Source = obj
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
