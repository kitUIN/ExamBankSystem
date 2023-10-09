using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExamBankSystem.Args;
using ExamBankSystem.Helpers;
using ExamBankSystem.Models;

namespace ExamBankSystem.ViewModels
{
    public partial class DataTableBase<T> : ObservableObject where T : OrderModel
    {
        /// <summary>
        /// 搜索的列
        /// </summary>
        protected string SearchCol;

        /// <summary>
        /// 列表
        /// </summary>
        public ObservableCollection<T> Items { get; } = new ObservableCollection<T>();

        /// <summary>
        /// 操作事件
        /// </summary>
        public event EventHandler<ActionEventArg> ActionEvent;

        public DataTableBase()
        {
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
        public async void Refresh()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                var items = await DbHelper.GetAsync<T>(CurrentPage);
                TotalPage = (items.Count + 14) / 15;
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
        public async void SearchRefresh(string text)
        {
            var items = await DbHelper.SearchAsync<T>(SearchCol, text, CurrentPage);
            TotalPage = (items.Count + 14) / 15;
            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
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
    }
}