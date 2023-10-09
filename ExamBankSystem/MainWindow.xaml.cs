using ExamBankSystem.Views;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;
using WinRT.Interop;

namespace ExamBankSystem
{
    public sealed partial class MainWindow : Window
    {
        public static AppWindow m_AppWindow;
        public MainWindow()
        {
            this.InitializeComponent();
            m_AppWindow = GetAppWindowForCurrentWindow();
            var titleBar = m_AppWindow.TitleBar;
            titleBar.ExtendsContentIntoTitleBar = true;
            ExtendsContentIntoTitleBar = true;
            this.SystemBackdrop = new MicaBackdrop();
        }
        public void Navigate()
        {
            ContentFrame.Navigate(typeof(MainView));
        }
        private AppWindow GetAppWindowForCurrentWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(wndId);
        }
    }
}
