using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
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
    public sealed partial class ExamSubjectTip : UserControl
    {
        public ExamSubjectTip()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// 显示
        /// </summary>
        public void Open()
        {
            AddTeachingTip.IsOpen = true;

        }
        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            AddTeachingTip.IsOpen = false;
        }
    }
}
