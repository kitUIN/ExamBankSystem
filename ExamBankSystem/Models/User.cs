using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamBankSystem.Models
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
