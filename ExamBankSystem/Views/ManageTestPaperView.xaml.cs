using ExamBankSystem.ViewModels;
using Windows.UI.Xaml.Controls;

namespace ExamBankSystem.Views
{
    public sealed partial class ManageTestPaperView : Page
    {
        public ManageTestPaperViewModel ViewModel = new ManageTestPaperViewModel();
        public ManageTestPaperView()
        {
            this.InitializeComponent();
        }
    }
}
