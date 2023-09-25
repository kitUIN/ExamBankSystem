using ExamBankSystem.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamBankSystem.Args
{
    public class ActionEventArg
    {
        public ActionMode ActionMode { get; set; }
        public object Source { get; set; }
    }
}
