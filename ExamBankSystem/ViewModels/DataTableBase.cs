using System;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExamBankSystem.Args;
using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using ExamBankSystem.Models;

namespace ExamBankSystem.ViewModels
{
    public partial class DataTableBase<T> : ObservableObject where T : OrderModel
    {
        /// <summary>
        /// 搜索的列
        /// </summary>
        public string SearchCol;

        /// <summary>
        /// 列表
        /// </summary>
        public ObservableCollection<T> Items { get; } = new ObservableCollection<T>();

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
        public DataTableBase(string searchCol)
        {
            SearchCol = searchCol;
            Refresh();
        }

        /// <summary>
        /// 搜索内容
        /// </summary>
        [ObservableProperty] private string searchText;

        /// <summary>
        /// 当前页码
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PrePageEnable))]
        [NotifyPropertyChangedFor(nameof(NextPageEnable))]
        private long currentPage = 1;

        /// <summary>
        /// 总页码
        /// </summary>
        [ObservableProperty] [NotifyPropertyChangedFor(nameof(NextPageEnable))]
        private long totalPage = 1;

        /// <summary>
        /// 前一页按钮启用
        /// </summary>
        public bool PrePageEnable => CurrentPage > 1;

        /// <summary>
        /// 后一页按钮启用
        /// </summary>
        public bool NextPageEnable => CurrentPage < TotalPage;

        /// <summary>
        /// 当前页改变时
        /// </summary>
        partial void OnCurrentPageChanged(long value)
        {
            Refresh();
        }

        /// <summary>
        /// 上一页
        /// </summary>
        [RelayCommand]
        private void PrePage()
        {
            CurrentPage--;
        }

        /// <summary>
        /// 下一页
        /// </summary>
        [RelayCommand]
        private void NextPage()
        {
            CurrentPage++;
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        public virtual async void Refresh()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                var items = await DbHelper.GetAsync<T>(CurrentPage);
                TotalPage = (DbHelper.GetCount<T>() + 14) / 15;
                Items.Clear();
                foreach (var item in items)
                {
                    Items.Add(item);
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
        public virtual async void SearchRefresh(string text)
        {
            var items = await DbHelper.SearchAsync<T>(SearchCol, text, CurrentPage);
            TotalPage = (DbHelper.GetCount<T>() + 14) / 15; 
            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        /// <summary>
        /// 搜索响应
        /// </summary>
        public virtual void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
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
        /// 添加多个按钮响应
        /// </summary> 
        [RelayCommand]
        private void AddMul()
        {
            ActionEvent?.Invoke(this, new ActionEventArg()
            {
                TipMode = TipMode.Show,
                ActionMode = ActionMode.AddMul,
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
        /// <summary>
        /// 重置按钮是否亮起
        /// </summary>
        [ObservableProperty] private bool canReset;
        /// <summary>
        /// 重置
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