using ExamBankSystem.Args;
using ExamBankSystem.Controls;
using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using ExamBankSystem.Utils;
using ExamBankSystem.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Threading.Tasks;

namespace ExamBankSystem.Views
{
    public sealed partial class MainView : Page
    {
        private MainViewModel ViewModel { get; set; } = new MainViewModel();
        public MainView()
        {
            this.InitializeComponent();
            App.m_window.SetTitleBar(AppTitleBar);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            EventHelper.TopGridEvent += Caller_TopGridEvent;
            EventHelper.LoginEvent += EventHelper_LoginEvent;
            EventHelper.LogoutEvent += EventHelper_LogoutEvent;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            EventHelper.TopGridEvent -= Caller_TopGridEvent;
            EventHelper.LoginEvent -= EventHelper_LoginEvent;
            EventHelper.LogoutEvent -= EventHelper_LogoutEvent;
        }

        private void EventHelper_LogoutEvent(object sender, EventArgs e)
        {
            CurrentData.CurrentUser = null;
            ViewModel.Logout();
            ConfigHelper.Set(ConfigKey.LastUserName, "");
            ConfigHelper.Set(ConfigKey.LastUserPassword, "");
            ContentFrame.Content = null;
            LoginTip.Show();
        }
        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            LoginTip.Show();
        }
        /// <summary>
        /// 点击左侧的导航项
        /// </summary>
        private void NavigationView_ItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(SettingsView));
                return;
            }

            if (args.InvokedItemContainer is NavigationViewItem { Tag: CategoryTag tag })
            {
                switch (tag)
                {
                    case CategoryTag.Login:
                        LoginTip.Show();
                        break;
                    case CategoryTag.ExamSubject:
                        ContentFrame.Navigate(typeof(ExamSubjectView));
                        break;
                    case CategoryTag.User:
                        ContentFrame.Navigate(typeof(UserView));
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
                    case CategoryTag.FindTestPaper:
                        ContentFrame.Navigate(typeof(FindTestPaperView));
                        break;
                    case CategoryTag.ManageTestPaper:
                        ContentFrame.Navigate(typeof(ManageTestPaperView));
                        break;
                    case CategoryTag.MergeTestPaper:
                        ContentFrame.Navigate(typeof(MergeTestPaperView));
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
        
    } }
}
