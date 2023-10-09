using ExamBankSystem.Enums;
namespace ExamBankSystem.Args
{
    public class ActionEventArg
    {
        public TipMode TipMode { get; set; } = TipMode.Show;
        public ActionMode ActionMode { get; set; }
        public object Source { get; set; }
    }
}
