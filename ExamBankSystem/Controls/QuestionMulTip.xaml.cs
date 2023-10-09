using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using ExamBankSystem.Models;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ExamBankSystem.Controls
{
    public sealed partial class QuestionMulTip : UserControl
    {
        /// <summary>
        /// 试卷
        /// </summary>
        private StorageFile paperFile; 
        /// <summary>
        /// 答案
        /// </summary>
        private StorageFile answerFile; 
        public QuestionMulTip()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 请求刷新事件
        /// </summary>
        public event EventHandler RefreshEvent;
        /// <summary>
        /// 显示
        /// </summary>
        public void Show()
        {
            MainTeachingTip.IsOpen = true;
            TestPapers.Content = ResourcesHelper.GetString(ResourceKey.SelectWord);
            Answer.Content = ResourcesHelper.GetString(ResourceKey.SelectWord);
            paperFile = null;
            answerFile = null;
            AddButton.IsEnabled = paperFile != null && answerFile != null;
        }
        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            MainTeachingTip.IsOpen = false;
        }

        /// <summary>
        /// 添加按钮响应
        /// </summary>
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            RefreshEvent?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// 回车响应
        /// </summary>
        private void Button_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                AddButton_Click(sender, e);
            }
        }

        private async void Answer_Click(object sender, RoutedEventArgs e)
        {
            answerFile = await FileHelper.GetDocxAsync();
            if (answerFile != null)
            {

                Answer.Content = answerFile.DisplayName;
            }
            AddButton.IsEnabled = paperFile != null && answerFile != null;
        }

        private async void TestPapers_Click(object sender, RoutedEventArgs e)
        {
            paperFile = await FileHelper.GetDocxAsync();
            if (paperFile != null)
            {
                TestPapers.Content = paperFile.DisplayName;
            }
            AddButton.IsEnabled = paperFile != null && answerFile != null;
        }
    }
}
