using ExamBankSystem.Models;

namespace ExamBankSystem.ViewModels
{
    public partial class KnowledgePointViewModel : DataTableBase<KnowledgePoint>
    {
        public KnowledgePointViewModel(string searchCol="name") : base(searchCol)
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