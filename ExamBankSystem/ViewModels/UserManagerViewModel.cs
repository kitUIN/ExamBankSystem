using ExamBankSystem.Models;

namespace ExamBankSystem.ViewModels
{
    public partial class UserManagerViewModel : DataTableBase<User>
    {
        public UserManagerViewModel(string searchCol="user") : base(searchCol)
        {
        }

        /// <summary>
        /// 列表选择变化响应
        /// </summary>
        public void SelectionChanged(int count)
        {
            CanDelete = CanReset = count > 0;
        }
    }
}