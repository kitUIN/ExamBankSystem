using ExamBankSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamBankSystem.Utils
{
    /// <summary>
    /// 程序运行时的数据
    /// </summary>
    public static class CurrentData
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        public static User CurrentUser { get; set; }
    }
}
