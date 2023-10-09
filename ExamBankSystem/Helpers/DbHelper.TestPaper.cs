using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamBankSystem.Helpers
{
    public partial class DbHelper
    {
        /// <summary>
        /// 创建试卷表
        /// </summary>
        private static void CreateTestPapersTable()
        {
            ExecuteReader(command =>
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS `TestPapers` (" +
                               "`id` INTEGER PRIMARY KEY AUTOINCREMENT , " +
                               "`name` NVARCHAR(2048) NOT NULL, " +
                               "`point` INTEGER NOT NULL, " +
                               "`isExamine` BOOLEAN NOT NULL, " +
                               "`uploadUser` NVARCHAR(2048) NOT NULL, " +
                               "`createTime` TIMESTAMP NOT NULL, " +
                               "`updateTime` TIMESTAMP NULL " +
                               ")";
                return command;
            }
            );
        }

    }
}
