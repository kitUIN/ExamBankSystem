using ExamBankSystem.Models;

namespace ExamBankSystem.ViewModels
{
    public partial class ExamSubjectViewModel : DataTableBase<ExamSubject>
    {

        public ExamSubjectViewModel(string searchCol = "subject") : base(searchCol)
        {

        }

        /// <summary>
        /// 列表选择变化响应
        /// </summary>
        public void SelectionChanged(int count)
        {
            CanEdit = count == 1;
            CanDelete = count > 0;
        }
    }
}