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
        /// 创建知识点表
        /// </summary>
        private static async void CreateKnowledgePointsTable(SqliteConnection db)
        {
            var tableCommand = "CREATE TABLE IF NOT EXISTS `KnowledgePoints` (" +
                               "`id` INTEGER PRIMARY KEY AUTOINCREMENT , " +
                               "`subjectId` INTEGER NOT NULL, " +
                               "`name` NVARCHAR(100) NOT NULL, " +
                               "`knowledge` NTEXT NOT NULL, " +
                               "`createTime` TIMESTAMP NOT NULL, " +
                               "`updateTime` TIMESTAMP NULL " +
                               ")";
            var createTable = new SqliteCommand(tableCommand, db);
            await createTable.ExecuteReaderAsync();
        }
        /// <summary>
        /// 从数据库中获取知识点
        /// </summary>
        public static async Task<List<KnowledgePoint>> GetKnowledgePointsAsync(long page = 1, int limit = 15)
        {
            return await GetAsync<KnowledgePoint>(page, limit);
        }

        /// <summary>
        /// 从数据库中获取知识点
        /// </summary>
        public static async Task<KnowledgePoint> GetKnowledgePointAsync(string name)
        {
            return await ExecuteReaderAsync(selectCommand =>
            {
                selectCommand.CommandText = $"SELECT * FROM `KnowledgePoints` WHERE `name` = @Name;";
                selectCommand.Parameters.AddWithValue("@Name", name);
                return selectCommand;
            }, query =>
            {
                while (query.Read())
                {
                    return new KnowledgePoint(query);
                }

                return null;
            });
        }

        /// <summary>
        /// 从数据库中获取知识点
        /// </summary>
        public static async Task<KnowledgePoint> GetKnowledgePointAsync(int id)
        {
            return await GetByIdAsync<KnowledgePoint>(id);
        }

        /// <summary>
        /// 从数据库中获取知识点
        /// </summary>
        public static KnowledgePoint GetKnowledgePoint(int id)
        {
            return GetById<KnowledgePoint>(id);
        }

        /// <summary>
        /// 插入知识点到数据库中
        /// </summary>
        public static void InsertKnowledgePoint(string name,string knowledge,int subject)
        {
            ExecuteReaderAsync(selectCommand =>
            {
                selectCommand.CommandText =
                    $"INSERT INTO `KnowledgePoints` VALUES (NULL, @Subject, @Name, @Knowledge, @CreateTime, @UpdateTime);";
                selectCommand.Parameters.AddWithValue("@Name", name);
                selectCommand.Parameters.AddWithValue("@Subject", subject);
                selectCommand.Parameters.AddWithValue("@Knowledge", knowledge);
                var t = DateTimeHelper.GetTimeStamp();
                selectCommand.Parameters.AddWithValue("@CreateTime", t);
                selectCommand.Parameters.AddWithValue("@UpdateTime", t);
                return selectCommand;
            });
        }

        /// <summary>
        /// 从数据库中删除知识点
        /// </summary>
        public static void DeleteKnowledgePoint(int id)
        {
            DeleteById<KnowledgePoint>(id);
        }

        /// <summary>
        /// 更新知识点名称
        /// </summary>
        public static void UpdateKnowledgePointName(int id, string newName)
        {
            ExecuteReaderAsync(selectCommand =>
            {
                selectCommand.CommandText =
                    $"UPDATE `KnowledgePoints` SET `name` = @Name, `updateTime` = @UpdateTime WHERE `id` = @ID;";
                selectCommand.Parameters.AddWithValue("@Name", newName);
                selectCommand.Parameters.AddWithValue("@UpdateTime", DateTimeHelper.GetTimeStamp());
                selectCommand.Parameters.AddWithValue("@ID", id);
                return selectCommand;
            });
        }

        /// <summary>
        /// 知识点下是否有问题
        /// </summary>
        public static async Task<bool> KnowledgePointHasAnyQuestionAsync(int id)
        {
            return (bool)await ExecuteScalarAsync(selectCommand =>
            {
                selectCommand.CommandText = "SELECT EXISTS(SELECT 1 FROM Questions WHERE subjectId = @ID);";
                selectCommand.Parameters.AddWithValue("@ID", id);
                return selectCommand;
            });
        }

        /// <summary>
        /// 搜索知识点
        /// </summary>
        public static async Task<List<KnowledgePoint>> SearchKnowledgePointAsync(string keyword, long page = 1,
            int limit = 15)
        {
            return await SearchAsync<KnowledgePoint>("subject", keyword, page, limit);
        }

    }
}