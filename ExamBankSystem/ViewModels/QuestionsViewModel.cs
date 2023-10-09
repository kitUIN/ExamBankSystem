using ExamBankSystem.Models;

namespace ExamBankSystem.ViewModels
{
    public partial class QuestionsViewModel : DataTableBase<Question>
    {
        public QuestionsViewModel(string searchCol="question") : base(searchCol)
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void SelectionChanged(int count)
        {
            CanDelete = CanEdit = count == 1;
        }
    }
}
