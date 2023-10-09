using ExamBankSystem.Models;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
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
        public static TestPaper GetTestPapersByName(string name)
        {
            return ExecuteReader(selectCommand =>
            {
                selectCommand.CommandText = $"SELECT * FROM `TestPapers` WHERE `name` = @ID;";
                selectCommand.Parameters.AddWithValue("@ID", name);
                return selectCommand;
            }, query =>
            {
                while (query.Read())
                {
                    return new TestPaper(query);
                }
                return null;
            });
        }
        public static void InsertTestPaper(string name, double point, bool isExamine, int uploadUserId)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();

                var insertCommand = db.CreateCommand();
                insertCommand.CommandText = $"INSERT INTO `TestPapers` VALUES VALUES (@Name, @Point, @IsExamine, @UploadUser, @CreateTime, @UpdateTime);";
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
        /// <summary>
        /// 获取是否需要审核的试卷
        /// </summary>
        public static List<TestPaper> GetIsExaminePaper(bool isExamine = false)
        {
            return ExecuteReader(selectCommand =>
            {
                selectCommand.CommandText = "SELECT * FROM `TestPapers` WHERE `isExamine` = @IsExamine;";
                selectCommand.Parameters.AddWithValue("@IsExamine", isExamine);
                return selectCommand;
            }, query =>
            {
                var res = new List<TestPaper>();
                while (query.Read())
                {
                    res.Add(new TestPaper(query));
                }
                return res;
            });
        }

        public static void UpdateTestPaper(int id, string col, object value)
        { 
            ExecuteReader(selectCommand =>
            {
                selectCommand.CommandText = $"UPDATE `TestPapers` SET {col} = @V WHERE `id` = @ID;";
                selectCommand.Parameters.AddWithValue("@V", value);
                selectCommand.Parameters.AddWithValue("@ID", id);
                return selectCommand;
            });
        }
    }
}
