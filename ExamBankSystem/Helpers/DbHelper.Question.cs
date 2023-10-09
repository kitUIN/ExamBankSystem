using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExamBankSystem.Enums;
using ExamBankSystem.Models;
using ExamBankSystem.Utils;
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
                    "subjectId = @Subject, type = @Type, " +
                    "rank = @Rank, " +
                    "knowledgeId = @KnowledgeName " +
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
        public static long GetQuestionCountAsync(string col, object keyword) 
        {
            return (long)ExecuteScalar(selectCommand =>
            {
                selectCommand.CommandText =
                    $"SELECT COUNT(*) FROM Questions WHERE {col} = @Keyword";
                selectCommand.Parameters.AddWithValue("@Keyword", keyword);
                return selectCommand;
            });
        }
        public static async Task<List<Question>> GetQuestionAsync(string col, object keyword) 
        { 
            return await ExecuteReaderAsync(selectCommand =>
            {
                selectCommand.CommandText =
                    $"SELECT * FROM Questions WHERE {col} = @Keyword;";
                selectCommand.Parameters.AddWithValue("@Keyword", keyword);
                return selectCommand;
            }, query =>
            {
                var res = new List<Question>();
                while (query.Read())
                {
                    var item = new Question( query);
                    
                    res.Add(item);
                }
                return res;
            });
        }
        public static async Task<List<Question>> GetQuestionAsync(string col, object keyword, long page ,
            int limit = 15) 
        {
            page--;
            return await ExecuteReaderAsync(selectCommand =>
            {
                selectCommand.CommandText =
                    $"SELECT * FROM Questions WHERE {col} = @Keyword LIMIT @Limit OFFSET @Offset;";
                selectCommand.Parameters.AddWithValue("@Keyword", keyword);
                selectCommand.Parameters.AddWithValue("@Limit", limit);
                selectCommand.Parameters.AddWithValue("@Offset", page * limit);
                return selectCommand;
            }, query =>
            {
                var order = page * limit;
                var res = new List<Question>();
                while (query.Read())
                {
                    var item = new Question( query);
                    item.Order = ++order;
                    res.Add(item);
                }

                return res;
            });
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

        public static async Task<bool> CheckQuestionPercent(string s1, int t)
        {
            foreach (var question in await GetQuestionAsync("type", t))
            {
                if (TextHelper.CheckText(s1, question.Name) > CurrentData.CheckedPercent)
                {
                    return true;
                }
            }
            return false;
        }
    }
}