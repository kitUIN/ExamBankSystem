using ExamBankSystem.Models;
using System.Collections.ObjectModel;

namespace ExamBankSystem.ViewModels
{
    public class FindTestPaperViewModel
    {
        public ObservableCollection<QuestionPaper> RightItems { get; } = new ObservableCollection<QuestionPaper>();
        public ObservableCollection<TestPaper> LeftItems { get; } = new ObservableCollection<TestPaper>();

    }
}
