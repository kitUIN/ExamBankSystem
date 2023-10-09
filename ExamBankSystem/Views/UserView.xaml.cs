using ExamBankSystem.ViewModels;
using Windows.UI.Xaml.Controls;

namespace ExamBankSystem.Views
{

    public sealed partial class UserView : Page
    {
        private UserViewModel ViewModel { get; } = new UserViewModel();
        public UserView()
        {
            this.InitializeComponent();
        }

    }
}
