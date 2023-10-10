using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamBankSystem.Models
{
    public partial class CheckWordModel:ObservableObject
    {
        [ObservableProperty]
        public Question left;
        [ObservableProperty]
        public Question right;
        [ObservableProperty]
        public double percent;
    }
}
