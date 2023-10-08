using System.Collections.Generic;
using ExamBankSystem.Enums;
using ExamBankSystem.Models;
using Microsoft.Data.Sqlite;

namespace ExamBankSystem.Helpers
{
    public partial class DbHelper
    {
        #region ExamSubject
        /// <summary>
        /// 创建考试科目表
        /// </summary>
        private static async void CreateExamSubjectsTable(SqliteConnection db)
        {
            var tableCommand = "CREATE TABLE IF NOT EXISTS `ExamSubjects` (" +
                               "`id` INT AUTO_INCREMENT  PRIMARY KEY, " +
                               "`subject` NVARCHAR(30) PRIMARY KEY, " +
                               "`createTime` TIMESTAMP NOT NULL, " +
                               "`updateTime` TIMESTAMP NULL " +
                               ")";
            var createTable = new SqliteCommand(tableCommand, db);
            await createTable.ExecuteReaderAsync();
        }
        /// <summary>
        /// 从数据库中获取考试科目
        /// </summary>
        public static List<ExamSubject> GetExamSubjects()
        {
            var res = new List<ExamSubject>(); 
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();
                var selectCommand = db.CreateCommand();
                selectCommand.CommandText = $"SELECT * FROM {DbTableName.ExamSubjects} ;";
                var query = selectCommand.ExecuteReader();
                while (query.Read())
                {
                    res.Add(ExamSubject.FromDb(query));
                }
            }
            return res;
        }
        /// <summary>
        /// 从数据库中获取考试科目
        /// </summary>
        public static ExamSubject GetExamSubject(string name)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();
                var selectCommand = db.CreateCommand();
                selectCommand.CommandText = $"SELECT * FROM {DbTableName.ExamSubjects} WHERE subject = @Name;";

                selectCommand.Parameters.AddWithValue("@Name", name);

                var query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    return ExamSubject.FromDb(query);
                }
            }
            return null;
        }
        /// <summary>
        /// 从数据库中获取考试科目
        /// </summary>
        public static ExamSubject GetExamSubject(int id)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();
                var selectCommand = db.CreateCommand();
                selectCommand.CommandText = $"SELECT * FROM {DbTableName.ExamSubjects} WHERE `id` = @Name;";

                selectCommand.Parameters.AddWithValue("@Name", id);

                var query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    return ExamSubject.FromDb(query);
                }
            }
            return null;
        }
        /// <summary>
        /// 插入考试科目到数据库中
        /// </summary>
        public static void InsertExamSubject(string name)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();

                var insertCommand = db.CreateCommand();
                insertCommand.CommandText = $"INSERT INTO {DbTableName.ExamSubjects} VALUES (@Name, @CreateTime, @UpdateTime);";
                insertCommand.Parameters.AddWithValue("@Name", name);
                var t = DateTimeHelper.GetTimeStamp();
                insertCommand.Parameters.AddWithValue("@CreateTime", t);
                insertCommand.Parameters.AddWithValue("@UpdateTime",t);
                insertCommand.ExecuteReader();
            }
        }
        /// <summary>
        /// 从数据库中删除考试科目
        /// </summary>
        public static void DeleteExamSubject(string key)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();

                var command = db.CreateCommand();
                command.CommandText = $"DELETE FROM {DbTableName.ExamSubjects} WHERE subject = @Name;";
                command.Parameters.AddWithValue("@Name", key);
                command.ExecuteReader();
            }
        }
        /// <summary>
        /// 更新科目名称
        /// </summary>
        public static void UpdateExamSubjectName(string newName,string oldName)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();

                var insertCommand = db.CreateCommand();
                insertCommand.CommandText = $"UPDATE {DbTableName.ExamSubjects} SET subject = @Name, updateTime = @UpdateTime WHERE subject = @oldName;";
                insertCommand.Parameters.AddWithValue("@Name", newName);
                insertCommand.Parameters.AddWithValue("@UpdateTime", DateTimeHelper.GetTimeStamp());
                insertCommand.Parameters.AddWithValue("@oldName", oldName);
                insertCommand.ExecuteReader();
            }
        }
        #endregion
    }
}