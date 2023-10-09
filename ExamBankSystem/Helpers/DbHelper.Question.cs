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
         private static async void CreateQuestionsTable(SqliteConnection db)
         {
             var tableCommand = "CREATE TABLE IF NOT EXISTS `Questions` (" +
                                "`id` INTEGER PRIMARY KEY AUTOINCREMENT , " +
                                "`subjectId` INT NOT NULL, " +
                                "`type` INT NOT NULL, " +
                                "`question` NTEXT NOT NULL, " +
                                "`choices` NTEXT NOT NULL, " +
                                "`answer` NTEXT NOT NULL, " +
                                "`point` DOUBLE NOT NULL, " +
                                "`rank` INT NOT NULL, " +
                                "`KnowledgeId` INT NOT NULL, " +
                                "`uploadUserId` INT NOT NULL, " +
                                "`createTime` TIMESTAMP NOT NULL, " +
                                "`updateTime` TIMESTAMP NULL " +
                                ")";
             var createTable = new SqliteCommand(tableCommand, db);
             await createTable.ExecuteReaderAsync();
         }
         /// <summary>
         /// 从数据库中获取题目
         /// </summary>
         public static async Task<List<Question>> GetQuestionsAsync(long page = 1, int limit = 15)
         {
             return await GetAsync<Question>(page, limit);
         }

         /// <summary>
         /// 从数据库中获取题目
         /// </summary>
         public static async Task<Question> GetQuestionAsync(int id)
         {
             return await GetByIdAsync<Question>(id);
         }

         /// <summary>
         /// 从数据库中获取题目
         /// </summary>
         public static Question GetQuestion(int id)
         {
             return GetById<Question>(id);
         }
        
        
        /// <summary>
        /// 插入题目到数据库中
        /// </summary>
        public static void InsertQuestion(Question instance)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();

                var insertCommand = db.CreateCommand();
                insertCommand.CommandText = $"INSERT INTO {DbTableName.Questions} (" +
                                            "subject, type, question, answer, point, rank, knowledgeName, " +
                                            "uploadUser, createTime, updateTime) " +
                                            "VALUES (@Subject, @Type, @Question, @Answer, " +
                                            "@Point, @Rank, @KnowledgeName, @UploadUser, @CreateTime, @UpdateTime);";
                insertCommand.Parameters.AddWithValue("@Subject", instance.SubjectId);
                insertCommand.Parameters.AddWithValue("@Type", instance.Type);
                insertCommand.Parameters.AddWithValue("@Question", instance.Name);
                insertCommand.Parameters.AddWithValue("@Answer", instance.Answer);
                insertCommand.Parameters.AddWithValue("@Point", instance.Point);
                insertCommand.Parameters.AddWithValue("@Rank", instance.Rank);
                insertCommand.Parameters.AddWithValue("@KnowledgeName", instance.KnowledgeId);
                insertCommand.Parameters.AddWithValue("@UploadUser", instance.UploadUserId);
                var t = DateTimeHelper.GetTimeStamp();
                insertCommand.Parameters.AddWithValue("@CreateTime", t);
                insertCommand.Parameters.AddWithValue("@UpdateTime", t);
                insertCommand.ExecuteReader();
            }
        }
        /// <summary>
        /// 更新题目
        /// </summary>
        public static void UpdateQuestion(Question obj, int key)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();

                var insertCommand = db.CreateCommand();
                insertCommand.CommandText = $"UPDATE {DbTableName.Questions} SET question = @Question, " +
                    "answer = @Answer, updateTime = @UpdateTime " +
                    "subject = @Subject, type = @Type " +
                    "point = @Point, rank = @Rank " +
                    "knowledgeName = @KnowledgeName " +
                    "WHERE id = @OldName;";
                insertCommand.Parameters.AddWithValue("@Subject", obj.SubjectId);
                insertCommand.Parameters.AddWithValue("@Type", (int)obj.Type);
                insertCommand.Parameters.AddWithValue("@Question", obj.Name);
                insertCommand.Parameters.AddWithValue("@Answer", obj.Answer);
                insertCommand.Parameters.AddWithValue("@Point", obj.Point);
                insertCommand.Parameters.AddWithValue("@Rank", obj.Rank);
                insertCommand.Parameters.AddWithValue("@KnowledgeName", obj.KnowledgeId);
                obj.UpdateTime = DateTime.Now;
                insertCommand.Parameters.AddWithValue("@UpdateTime", DateTimeHelper.ToTimeStamp(obj.UpdateTime));
                insertCommand.Parameters.AddWithValue("@OldName", key);
                insertCommand.ExecuteReader();
            }
        }
        /// <summary>
        /// 搜索题目
        /// </summary>
        public static async Task<List<Question>> SearchQuestionAsync(string keyword, long page = 1,
            int limit = 15)
        {
            return await SearchAsync<Question>("title", keyword, page, limit);
        }

    }
}