using ExamBankSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamBankSystem.ViewModels
{
    public class TestPaperViewModel
    {
        public ObservableCollection<TestPaper> TestPapers { get; set; } = new ObservableCollection<TestPaper>();

    }
}
