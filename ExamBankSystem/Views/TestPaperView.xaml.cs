using ExamBankSystem.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace ExamBankSystem.Views
{
    public sealed partial class TestPaperView : Page
    {
        private TestPaperViewModel ViewModel { get; } =  new TestPaperViewModel();
        public TestPaperView()
        {
            this.InitializeComponent();
        }
    }
}
