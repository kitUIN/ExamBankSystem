using ExamBankSystem.Args;
using ExamBankSystem.Controls;
using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using ExamBankSystem.Models;
using ExamBankSystem.ViewModels;
using ExamBankSystem.Views;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using NavigationViewItem = Microsoft.UI.Xaml.Controls.NavigationViewItem;

namespace ExamBankSystem
{
    /// <summary>
    /// 主页面
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel { get; } = new MainViewModel();
        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.SetTitleBar(AppTitleBar);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            EventHelper.TopGridEvent += Caller_TopGridEvent;
            EventHelper.LoginEvent += EventHelper_LoginEvent;
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            EventHelper.TopGridEvent -= Caller_TopGridEvent;
            EventHelper.LoginEvent -= EventHelper_LoginEvent;
        }
        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            LoginTip.Open();
        }
        /// <summary>
        /// 点击左侧的导航项
        /// </summary>
        private void NavigationView_ItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            if(args.InvokedItemContainer is NavigationViewItem  item && item.Tag is CategoryTag tag)
            {
                switch (tag)
                {
                    case CategoryTag.Login:
                        LoginTip.Open();
                        break;
                    case CategoryTag.ExamSubject:
                        ContentFrame.Navigate(typeof(ExamSubjectView));
                        break;
                    case CategoryTag.User:
                        break;
                    case CategoryTag.UserManager:
                        ContentFrame.Navigate(typeof(UserManagerView));
                        break;
                    case CategoryTag.KnowledgePoint:
                        ContentFrame.Navigate(typeof(KnowledgePointView));
                        break;
                    case CategoryTag.Question:
                        ContentFrame.Navigate(typeof(QuestionView));
                        break;
                    case CategoryTag.QuestionPaper:
                        // ContentFrame.Navigate(typeof(KnowledgePointView));
                        break;
                    case CategoryTag.ManageTestPaper:
                        ContentFrame.Navigate(typeof(TestPaperView));
                        break;
                }
            }
        }

        /// <summary>
        /// 顶部窗体事件
        /// </summary>
        private async void Caller_TopGridEvent(object sender, TopGridEventArg e)
        {
            try
            {
                switch (e.Mode)
                {
                    case TopGridMode.ContentDialog:
                        if (e.Element is ContentDialog dialog)
                        {
                            dialog.XamlRoot = XamlRoot;
                            await dialog.ShowAsync();
                        }
                        break;
                    case TopGridMode.Dialog:
                        
                        break;
                    case TopGridMode.Tip:
                        if (e.Element is TipPopup popup)
                        {
                            TipContainer.Visibility = Visibility.Visible;
                            TipContainer.Children.Add(popup);
                            popup.Visibility = Visibility.Visible;
                            await Task.Delay(TimeSpan.FromSeconds(popup.DisplaySeconds));
                            popup.Visibility = Visibility.Collapsed;
                            TipContainer.Children.Remove(popup);
                            if (TipContainer.Children.Count == 0)
                            {
                                TipContainer.Visibility = Visibility.Collapsed;
                            }
                        }

                        break;
                }
            }
            catch (Exception)
            {
                 
            }
        }
        /// <summary>
        /// 登录通知
        /// </summary>
        private void EventHelper_LoginEvent(object sender, EventArgs e)
        {
            ViewModel.Init();
        }
    }
}
