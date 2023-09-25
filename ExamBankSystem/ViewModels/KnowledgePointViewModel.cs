using ExamBankSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamBankSystem.ViewModels
{
    public class KnowledgePointViewModel
    { 
        /// <summary>
        /// 知识点列表
        /// </summary>
        public ObservableCollection<KnowledgePoint> KnowledgePoints { get; set; } = new ObservableCollection<KnowledgePoint>();

    }
}
