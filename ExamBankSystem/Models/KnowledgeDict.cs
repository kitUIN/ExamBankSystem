using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamBankSystem.Models
{
    public class KnowledgeDict
    {
        public KnowledgeDict(KeyValuePair<string, int> kv)
        {
            Name = kv.Key;
            Count = kv.Value;
        }
        public string Name { get; set; }
        public int Count { get; set; } 
    }
}
