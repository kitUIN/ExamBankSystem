using ExamBankSystem.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ExamBankSystem.Controls
{
    public sealed partial class QuestionTip : UserControl
    {
        /// <summary>
        /// 请求刷新事件
        /// </summary>
        public event EventHandler RefreshEvent;

        public QuestionTip()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void Show(ActionMode mode, object obj = null)
        {
            switch (mode)
            {
                case ActionMode.Add:
                    MainTeachingTip.IsOpen = true;
                    break;
                case ActionMode.Edit:
                    break;
            }
        }
        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            MainTeachingTip.IsOpen = false;
        }
    }
}
