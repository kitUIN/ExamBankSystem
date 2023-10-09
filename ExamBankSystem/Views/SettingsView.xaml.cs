using System;
using Windows.Storage;
using Windows.System;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;

namespace ExamBankSystem.Views
{
    public sealed partial class SettingsView : Page
    {
        public SettingsView()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchFolderAsync(ApplicationData.Current.LocalFolder);
        }
    }
}
