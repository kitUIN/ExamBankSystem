using ExamBankSystem.ViewModels;
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


namespace ExamBankSystem.Views
{
    /// <summary>
    /// 考试科目页
    /// </summary>
    public sealed partial class ExamSubjectView : Page
    {
        private ExamSubjectViewModel ViewModel { get; } = new ExamSubjectViewModel();
        public ExamSubjectView()
        {
            this.InitializeComponent();
        }
    }
}
