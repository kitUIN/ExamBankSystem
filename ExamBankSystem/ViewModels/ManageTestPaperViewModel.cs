using ExamBankSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamBankSystem.ViewModels
{
    public partial class ManageTestPaperViewModel : DataTableBase<TestPaper>
    {
        public ManageTestPaperViewModel(string searchCol = "") : base(searchCol)
        {
        }

    }
}
