using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using Windows.Storage;
using Windows.System;

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
        private void AddButton_Click(object sender, RoutedEventArgs e)
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
