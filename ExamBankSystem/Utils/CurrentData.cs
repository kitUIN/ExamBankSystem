using ExamBankSystem.Models;

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

        public static double CheckedPercent { get; } = 0.95;
    }
}
