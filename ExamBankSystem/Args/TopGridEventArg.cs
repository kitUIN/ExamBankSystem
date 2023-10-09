using ExamBankSystem.Enums;
using Microsoft.UI.Xaml;

namespace ExamBankSystem.Args
{
    public class TopGridEventArg
    {
        public UIElement Element { get; set; }
        public TopGridMode Mode { get; set; }

        public TopGridEventArg(UIElement element, TopGridMode mode)
        {
            Element = element;
            Mode = mode;
        }
    }
}

