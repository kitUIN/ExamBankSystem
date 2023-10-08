using System.Collections.Generic;
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
                               "`name` NVARCHAR(100), " +
                               "`knowledge` NTEXT NOT NULL, " +
                               "`createTime` TIMESTAMP NOT NULL, " +
                               "`updateTime` TIMESTAMP NULL " +
                               ")";
            var createTable = new SqliteCommand(tableCommand, db);
            await createTable.ExecuteReaderAsync();
        }
         #region KnowledgePoint
        /// <summary>
        /// 从数据库中获取知识点
        /// </summary>
        public static List<KnowledgePoint> GetKnowledgePoints()
        {
            var res = new List<KnowledgePoint>();
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();
                var selectCommand = db.CreateCommand();
                selectCommand.CommandText = $"SELECT * FROM {DbTableName.KnowledgePoints} ;";
                var query = selectCommand.ExecuteReader();
                while (query.Read())
                {
                    res.Add(KnowledgePoint.FromDb(query));
                }
            }
            return res;
        }
        /// <summary>
        /// 从数据库中获取知识点
        /// </summary>
        public static KnowledgePoint GetKnowledgePoint(string name)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();
                var selectCommand = db.CreateCommand();
                selectCommand.CommandText = $"SELECT * FROM {DbTableName.KnowledgePoints} WHERE name = @Name;";

                selectCommand.Parameters.AddWithValue("@Name", name);

                var query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    return KnowledgePoint.FromDb(query);
                }
            }
            return null;
        }
        /// <summary>
        /// 从数据库中获取知识点
        /// </summary>
        public static KnowledgePoint GetKnowledgePoint(int id)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();
                var selectCommand = db.CreateCommand();
                selectCommand.CommandText = $"SELECT * FROM {DbTableName.KnowledgePoints} WHERE `id` = @Name;";

                selectCommand.Parameters.AddWithValue("@Name", id);

                var query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    return KnowledgePoint.FromDb(query);
                }
            }
            return null;
        }
        /// <summary>
        /// 插入知识点到数据库中
        /// </summary>
        public static void InsertKnowledgePoint(KnowledgePoint instance)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();

                var insertCommand = db.CreateCommand();
                insertCommand.CommandText = $"INSERT INTO {DbTableName.KnowledgePoints} VALUES (@Name, @Content, @CreateTime, @UpdateTime);";
                insertCommand.Parameters.AddWithValue("@Name", instance.Name);
                var t = DateTimeHelper.GetTimeStamp();
                insertCommand.Parameters.AddWithValue("@Content", instance.Knowledge);
                insertCommand.Parameters.AddWithValue("@CreateTime", t);
                insertCommand.Parameters.AddWithValue("@UpdateTime", t);
                insertCommand.ExecuteReader();
            }
        }
        /// <summary>
        /// 从数据库中删除知识点
        /// </summary>
        public static void DeleteKnowledgePoint(string key)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();

                var command = db.CreateCommand();
                command.CommandText = $"DELETE FROM {DbTableName.KnowledgePoints} WHERE name = @Name;";
                command.Parameters.AddWithValue("@Name", key);
                command.ExecuteReader();
            }
        }
        /// <summary>
        /// 更新科目
        /// </summary>
        public static void UpdateKnowledgePoint(KnowledgePoint obj, string key)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();

                var insertCommand = db.CreateCommand();
                insertCommand.CommandText = $"UPDATE {DbTableName.KnowledgePoints} SET name = @Name, knowledge = @Knowledge, updateTime = @UpdateTime WHERE name = @OldName;";
                insertCommand.Parameters.AddWithValue("@Name", obj.Name);
                insertCommand.Parameters.AddWithValue("@Knowledge", obj.Knowledge);
                insertCommand.Parameters.AddWithValue("@UpdateTime", DateTimeHelper.GetTimeStamp());
                insertCommand.Parameters.AddWithValue("@OldName", key);
                insertCommand.ExecuteReader();
            }
        }

        #endregion

    }
}