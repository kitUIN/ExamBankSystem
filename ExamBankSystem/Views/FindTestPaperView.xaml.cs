using ExamBankSystem.Models;
using ExamBankSystem.ViewModels;
using Windows.UI.Xaml.Controls;

namespace ExamBankSystem.Views
{

    public sealed partial class FindTestPaperView : Page
    {
        private FindTestPaperViewModel ViewModel { get; } = new FindTestPaperViewModel();

        public FindTestPaperView()
        {
            this.InitializeComponent();
        }

        private void LeftList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.LeftClick(e.ClickedItem as TestPaper);
        }
    }
}
