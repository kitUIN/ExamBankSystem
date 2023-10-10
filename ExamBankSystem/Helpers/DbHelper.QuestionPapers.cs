using ExamBankSystem.Models;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace ExamBankSystem.Helpers
{
    public partial class DbHelper
    {
        /// <summary>
        /// 创建试卷题目链接表
        /// </summary>
        private static void CreateQuestionPapersTable()
        {
            ExecuteReader(command =>
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS `QuestionPapers` (" +
                               "`id` INTEGER PRIMARY KEY AUTOINCREMENT , " +
                               "`testPaperId` INTEGER NOT NULL, " +
                               "`questionIndex` INTEGER NOT NULL, " +
                               "`questionId` INTEGER NOT NULL, " +
                               "`uploadUserId` INTEGER NOT NULL, " +
                               "`createTime` TIMESTAMP NOT NULL, " +
                               "`updateTime` TIMESTAMP NULL " +
                               ")";
                return command;
            }
            );
        } 
        public static List<QuestionPaper> GetQuestionsPapersByTestPaper(int id)
        {
            return ExecuteReader(selectCommand =>
            {
                selectCommand.CommandText = $"SELECT * FROM `QuestionPapers` WHERE `testPaperId` = @TestPaperId;";
                selectCommand.Parameters.AddWithValue("@TestPaperId", id);
                return selectCommand;
            }, query =>
            {
                var res = new List<QuestionPaper>();
                while (query.Read())
                {
                    res.Add(new QuestionPaper(query));
                }

                return res;
            });
        }
         
        public static QuestionPaper GetQuestionsPapers(int id, int questionId)
        {
            return ExecuteReader(selectCommand =>
            {
                selectCommand.CommandText = $"SELECT * FROM `QuestionPapers` WHERE `testPaperId` = @TestPaperId AND questionId = @QuestionId;";
                selectCommand.Parameters.AddWithValue("@TestPaperId", id);
                selectCommand.Parameters.AddWithValue("@QuestionId", questionId);
                return selectCommand;
            }, query =>
            { 
                while (query.Read())
                {
                    return new QuestionPaper(query);
                }

                return null;
            });
        }
        public static void InsertQuestionPaper(int testPaperId, int index, int questionId, int uploadUserId)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();

                var insertCommand = db.CreateCommand();
                insertCommand.CommandText = $"INSERT INTO `QuestionPapers`  VALUES (NULL, @TestPaperId, @QuestionIndex, @QuestionId, @UploadUser, @CreateTime, @UpdateTime);";
                insertCommand.Parameters.AddWithValue("@TestPaperId", testPaperId);
                insertCommand.Parameters.AddWithValue("@QuestionIndex", index);
                insertCommand.Parameters.AddWithValue("@QuestionId", questionId);
                insertCommand.Parameters.AddWithValue("@UploadUser", uploadUserId);
                var t = DateTimeHelper.GetTimeStamp();
                insertCommand.Parameters.AddWithValue("@CreateTime", t);
                insertCommand.Parameters.AddWithValue("@UpdateTime", t);
                insertCommand.ExecuteReader();
            }
        }
        public static void UpdateQuestionPaper(int testPaperId, int index, int questionId)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();
                var insertCommand = db.CreateCommand();
                insertCommand.CommandText = $"UPDATE  `QuestionPapers` SET questionId = @QuestionId Where testPaperId =  @TestPaperId AND questionIndex = @QuestionIndex;";
                insertCommand.Parameters.AddWithValue("@TestPaperId", testPaperId);
                insertCommand.Parameters.AddWithValue("@QuestionIndex", index);
                insertCommand.Parameters.AddWithValue("@QuestionId", questionId);
                var t = DateTimeHelper.GetTimeStamp();
                insertCommand.Parameters.AddWithValue("@UpdateTime", t);
                insertCommand.ExecuteReader();
            }
        }
        public static void DeleteQuestionPaperByTestPaperId(int testPaperId)
        {
            ExecuteReader(selectCommand =>
            { 
                selectCommand.CommandText = $"DELETE FROM `QuestionPapers` WHERE `testPaperId` = @Name;";
                selectCommand.Parameters.AddWithValue("@Name", testPaperId);
                return selectCommand;
            });
        }
    }
}
