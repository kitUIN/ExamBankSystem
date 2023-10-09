using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExamBankSystem.Enums;
using ExamBankSystem.Models;
using Microsoft.Data.Sqlite;

namespace ExamBankSystem.Helpers
{
    public partial class DbHelper
    {
        /// <summary>
        /// 创建题目表
        /// </summary>
        private static void CreateQuestionsTable()
         {
            ExecuteReader(command =>
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS `Questions` (" +
                                "`id` INTEGER PRIMARY KEY AUTOINCREMENT , " +
                                "`subjectId` INT NOT NULL, " +
                                "`type` INT NOT NULL, " +
                                "`question` NTEXT NOT NULL, " +
                                "`point` REAL NOT NULL, " +
                                "`answer` NTEXT NOT NULL, " +
                                "`rank` INT NOT NULL, " +
                                "`knowledgeId` INT NOT NULL, " +
                                "`uploadUserId` INT NOT NULL, " +
                                "`createTime` TIMESTAMP NOT NULL, " +
                                "`updateTime` TIMESTAMP NULL " +
                                ")";
                return command;
            }
            );
         }

        /// <summary>
        /// 插入题目到数据库中
        /// </summary>
        public static void InsertQuestion(int subjectId,int mode,string name,double point, string answer,
            int rank, int knowledgeId,int uploadUserId)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();

                var insertCommand = db.CreateCommand();
                insertCommand.CommandText = $"INSERT INTO `Questions` "+
                                            "VALUES (NULL, @Subject, @Type, @Question, @Choices, @Answer, " +
                                            " @Rank, @KnowledgeName, @UploadUser, @CreateTime, @UpdateTime);";
                insertCommand.Parameters.AddWithValue("@Subject", subjectId);
                insertCommand.Parameters.AddWithValue("@Type", mode);
                insertCommand.Parameters.AddWithValue("@Question", name);
                insertCommand.Parameters.AddWithValue("@Choices", point);
                insertCommand.Parameters.AddWithValue("@Answer", answer);
                insertCommand.Parameters.AddWithValue("@Rank", rank);
                insertCommand.Parameters.AddWithValue("@KnowledgeName", knowledgeId);
                insertCommand.Parameters.AddWithValue("@UploadUser", uploadUserId);
                var t = DateTimeHelper.GetTimeStamp();
                insertCommand.Parameters.AddWithValue("@CreateTime", t);
                insertCommand.Parameters.AddWithValue("@UpdateTime", t);
                insertCommand.ExecuteReader();
            }
        }
        /// <summary>
        /// 更新题目
        /// </summary>
        public static void UpdateQuestion(int id,int subjectId, int mode, string name, double point, string answer,
            int rank, int knowledgeId, int uploadUserId)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();

                var insertCommand = db.CreateCommand();
                insertCommand.CommandText = $"UPDATE `Questions` SET question = @Question, " +
                    "answer = @Answer, updateTime = @UpdateTime, " +
                    "subject = @Subject, type = @Type, " +
                    "rank = @Rank, " +
                    "knowledgeName = @KnowledgeName " +
                    "WHERE id = @ID;";
                insertCommand.Parameters.AddWithValue("@Subject", subjectId);
                insertCommand.Parameters.AddWithValue("@Type", mode);
                insertCommand.Parameters.AddWithValue("@Question", name);
                insertCommand.Parameters.AddWithValue("@Choices", point);
                insertCommand.Parameters.AddWithValue("@Answer", answer);
                insertCommand.Parameters.AddWithValue("@Rank", rank);
                insertCommand.Parameters.AddWithValue("@KnowledgeName", knowledgeId);
                insertCommand.Parameters.AddWithValue("@UploadUser", uploadUserId);
                insertCommand.Parameters.AddWithValue("@UpdateTime", DateTimeHelper.ToTimeStamp(DateTime.Now));
                insertCommand.Parameters.AddWithValue("@ID", id);
                insertCommand.ExecuteReader();
            }
        }
        /// <summary>
        /// 题目是否被任何试卷采用
        /// </summary>
        public static async Task<bool> QuestionHasAnyTestPaperAsync(int questionId)
        {
            return ((long)await ExecuteScalarAsync(selectCommand =>
            {
                selectCommand.CommandText = "SELECT COUNT(*) FROM QuestionPapers WHERE questionId = @ID;";
                selectCommand.Parameters.AddWithValue("@ID", questionId);
                return selectCommand;
            })) > 0;
        }
    }
}