using ExamBankSystem.Models;

namespace ExamBankSystem.ViewModels
{
    public partial class TestPaperViewModel : DataTableBase<TestPaper>
    {
        public TestPaperViewModel(string searchCol="") : base(searchCol)
        {
        }
    }
}
