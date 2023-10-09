using ExamBankSystem.Enums;
using ExamBankSystem.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
                               "`uploadUserId` INTEGER NOT NULL, " +
                               "`createTime` TIMESTAMP NOT NULL, " +
                               "`updateTime` TIMESTAMP NULL " +
                               ")";
                return command;
            }
            );
        }
        /// <summary>
        /// 获取用户的试卷
        /// </summary>
        public static List<TestPaper> GetTestPapersByUser(int id)
        {
            return ExecuteReader(selectCommand =>
            {
                selectCommand.CommandText = $"SELECT * FROM `TestPapers` WHERE `uploadUserId` = @ID;";
                selectCommand.Parameters.AddWithValue("@ID", id);
                return selectCommand;
            }, query =>
            {
                var res =new List<TestPaper>();
                while (query.Read())
                {
                    res.Add( new TestPaper(query));
                }

                return res;
            });
        }
        public static void InsertTestPaper(string name, double point, bool isExamine, int uploadUserId)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();

                var insertCommand = db.CreateCommand();
                insertCommand.CommandText = $"INSERT INTO `TestPapers` VALUES (name, point, isExamine, uploadUser, createTime, updateTime) VALUES (@Name, @Point, @IsExamine, @UploadUser, @CreateTime, @UpdateTime);";
                insertCommand.Parameters.AddWithValue("@Name", name);
                insertCommand.Parameters.AddWithValue("@Point", point);
                insertCommand.Parameters.AddWithValue("@IsExamine", isExamine);
                insertCommand.Parameters.AddWithValue("@UploadUser", uploadUserId);
                var t = DateTimeHelper.GetTimeStamp();
                insertCommand.Parameters.AddWithValue("@CreateTime", t);
                insertCommand.Parameters.AddWithValue("@UpdateTime", t);
                insertCommand.ExecuteReader();
            }
        }
    }
}
