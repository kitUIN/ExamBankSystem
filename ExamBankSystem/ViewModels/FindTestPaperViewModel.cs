using ExamBankSystem.Helpers;
using ExamBankSystem.Models;
using ExamBankSystem.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ExamBankSystem.ViewModels
{
    public class FindTestPaperViewModel
    {
        public ObservableCollection<QuestionPaper> RightItems { get; } = new ObservableCollection<QuestionPaper>();
        public ObservableCollection<TestPaper> LeftItems { get; } = new ObservableCollection<TestPaper>();

        public FindTestPaperViewModel()
        {
            RefreshLeft();
        }
        public void RefreshLeft()
        {
            LeftItems.Clear();
            foreach (var item in DbHelper.GetTestPapersByUser(CurrentData.CurrentUser.Id))
            {
                LeftItems.Add(item);
            }
        }
        /// <summary>
        /// 点击试卷
        /// </summary>
        public void LeftClick(TestPaper paper)
        {
            Debug.WriteLine(paper.Id);
            RightItems.Clear();
            foreach (var item in DbHelper.GetQuestionsPapersByTestPaper(paper.Id))
            {
                RightItems.Add(item);
            }
        }
    }
}
