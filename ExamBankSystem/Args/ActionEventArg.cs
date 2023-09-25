using ExamBankSystem.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Spi;

namespace ExamBankSystem.Args
{
    public class ActionEventArg
    {
        public TipMode TipMode { get; set; } = TipMode.Show;
        public ActionMode ActionMode { get; set; }
        public object Source { get; set; }
    }
}
