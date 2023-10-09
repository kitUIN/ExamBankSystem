using ExamBankSystem.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace ExamBankSystem.Views
{
    public sealed partial class MergeTestPaperView : Page
    {
        private MergeTestPaperViewModel ViewModel { get; set; } = new MergeTestPaperViewModel();
        public MergeTestPaperView()
        {
            this.InitializeComponent();
        }
    }
}
