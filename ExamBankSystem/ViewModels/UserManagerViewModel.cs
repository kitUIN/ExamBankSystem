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
    public partial class UserManagerViewModel : DataTableBase
    {
        /// <summary>
        /// 操作事件
        /// </summary>
        public event EventHandler<ActionEventArg> ActionEvent;

        /// <summary>
        /// 重置按钮是否亮起
        /// </summary>
        [ObservableProperty] private bool canReset = false;

        /// <summary>
        /// 删除按钮是否亮起
        /// </summary>
        [ObservableProperty] private bool canDelete = false;

        
        /// <summary>
        /// 用户列表
        /// </summary>
        public ObservableCollection<User> Users { get; } = new ObservableCollection<User>();

        /// <summary>
        /// 刷新列表
        /// </summary>
        public override async void Refresh()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                var items = await DbHelper.GetUsersAsync(CurrentPage);
                TotalPage = (items.Count + 14) / 15;
                Users.Clear();
                foreach (var item in items)
                {
                    Users.Add(item);
                }
            }
            else
            {
                SearchRefresh(SearchText);
            }
        }
        /// <summary>
        /// 搜索刷新
        /// </summary>
        /// <param name="text"></param>
        public async void SearchRefresh(string text)
        {
            var items = await DbHelper.SearchUserAsync(text, CurrentPage);
            TotalPage = (items.Count + 14) / 15;
            Users.Clear();
            foreach (var item in items)
            {
                Users.Add(item);
            }
        }
        /// <summary>
        /// 搜索响应
        /// </summary>
        public void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox box)
            {
                CurrentPage = 1;
                TotalPage = 1;
                if (string.IsNullOrEmpty(box.Text))
                {
                    Refresh();
                }
                else
                {
                    SearchRefresh(box.Text);
                }
            }
        }
        
        /// <summary>
        /// 列表选择变化响应
        /// </summary>
        public void SelectionChanged(int count)
        {
            CanDelete = CanReset = count > 0;
        }
        
        /// <summary>
        /// 添加
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
        /// <summary>
        /// 删除
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