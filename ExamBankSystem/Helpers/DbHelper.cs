using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.Data.Sqlite;
using ExamBankSystem.Models;
using ExamBankSystem.Enums;
using Windows.UI.Xaml.Shapes;
using Path = System.IO.Path;

namespace ExamBankSystem.Helpers
{
    /// <summary>
    /// 数据库帮助类
    /// </summary>
    public static class DbHelper
    {
        private static string _dbpath;

        #region Initialize

        /// <summary>
        /// 初始化数据库
        /// </summary>
        public static async void InitializeDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("bank.sqlite",
                CreationCollisionOption.OpenIfExists);
            _dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "bank.sqlite");
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();
                CreateUsersTable(db);
                CreateKnowledgePointsTable(db);
                CreateExamSubjectsTable(db);
                CreateQuestionsTable(db);
                CreateTestPapersTable(db);
                CreateQuestionPapersTable(db);
            }
        }

        /// <summary>
        /// 创建用户表
        /// </summary>
        private static async void CreateUsersTable(SqliteConnection db)
        {
            var tableCommand = "CREATE TABLE IF NOT EXISTS `Users` (" +
                               "`user` NVARCHAR(2048) PRIMARY KEY, " +
                               "`password` NVARCHAR(2048) NOT NULL, " +
                               "`role` NVARCHAR(2048) NOT NULL, " +
                               "`createTime` TIMESTAMP NOT NULL, " +
                               "`lastLoginTime` TIMESTAMP NULL " +
                               ")";
            var createTable = new SqliteCommand(tableCommand, db);
            await createTable.ExecuteReaderAsync();
        }

        /// <summary>
        /// 创建知识点表
        /// </summary>
        private static async void CreateKnowledgePointsTable(SqliteConnection db)
        {
            var tableCommand = "CREATE TABLE IF NOT EXISTS `KnowledgePoints` (" +
                               "`name` NVARCHAR(2048) PRIMARY KEY, " +
                               "`knowledge` NVARCHAR(2048) NOT NULL, " +
                               "`createTime` TIMESTAMP NOT NULL, " +
                               "`updateTime` TIMESTAMP NULL " +
                               ")";
            var createTable = new SqliteCommand(tableCommand, db);
            await createTable.ExecuteReaderAsync();
        }

        /// <summary>
        /// 创建考试科目表
        /// </summary>
        private static async void CreateExamSubjectsTable(SqliteConnection db)
        {
            var tableCommand = "CREATE TABLE IF NOT EXISTS `ExamSubjects` (" +
                               "`subject` NVARCHAR(2048) PRIMARY KEY, " +
                               "`createTime` TIMESTAMP NOT NULL, " +
                               "`updateTime` TIMESTAMP NULL " +
                               ")";
            var createTable = new SqliteCommand(tableCommand, db);
            await createTable.ExecuteReaderAsync();
        }

        /// <summary>
        /// 创建题目表
        /// </summary>
        private static async void CreateQuestionsTable(SqliteConnection db)
        {
            var tableCommand = "CREATE TABLE IF NOT EXISTS `Questions` (" +
                               "`id` INT AUTO_INCREMENT  PRIMARY KEY, " +
                               "`subject` NVARCHAR(2048) NOT NULL, " +
                               "`type` INT NOT NULL, " +
                               "`question` NTEXT NOT NULL, " +
                               "`answer` NTEXT NOT NULL, " +
                               "`point` DOUBLE NOT NULL, " +
                               "`rank` INT NOT NULL, " +
                               "`knowledgeName` NVARCHAR(2048) NOT NULL, " +
                               "`uploadUser` NVARCHAR(2048) NOT NULL, " +
                               "`createTime` TIMESTAMP NOT NULL, " +
                               "`updateTime` TIMESTAMP NULL " +
                               ")";
            var createTable = new SqliteCommand(tableCommand, db);
            await createTable.ExecuteReaderAsync();
        }

        /// <summary>
        /// 创建试卷表
        /// </summary>
        private static async void CreateTestPapersTable(SqliteConnection db)
        {
            var tableCommand = "CREATE TABLE IF NOT EXISTS `TestPapers` (" +
                               "`id` INT AUTO_INCREMENT  PRIMARY KEY, " +
                               "`name` NVARCHAR(2048) NOT NULL, " +
                               "`point` INT NOT NULL, " +
                               "`isExamine` BOOLEAN NOT NULL, " +
                               "`uploadUser` NVARCHAR(2048) NOT NULL, " +
                               "`createTime` TIMESTAMP NOT NULL, " +
                               "`updateTime` TIMESTAMP NULL " +
                               ")";
            var createTable = new SqliteCommand(tableCommand, db);
            await createTable.ExecuteReaderAsync();
        }

        /// <summary>
        /// 创建试卷题目链接表
        /// </summary>
        private static async void CreateQuestionPapersTable(SqliteConnection db)
        {
            var tableCommand = "CREATE TABLE IF NOT EXISTS `QuestionPapers` (" +
                               "`id` INT AUTO_INCREMENT  PRIMARY KEY, " +
                               "`testPaperId` INT NOT NULL, " +
                               "`questionIndex` INT NOT NULL, " +
                               "`questionId` INT NOT NULL, " +
                               "`uploadUser` NVARCHAR(2048) NOT NULL, " +
                               "`createTime` TIMESTAMP NOT NULL, " +
                               "`updateTime` TIMESTAMP NULL " +
                               ")";
            var createTable = new SqliteCommand(tableCommand, db);
            await createTable.ExecuteReaderAsync();
        }

        #endregion
        
        /// <summary>
        /// 从数据库中获取用户
        /// </summary>
        /// <param name="user">用户名</param>
        public static User GetUser(string user)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();
                SqliteCommand selectCommand = db.CreateCommand();
                selectCommand.CommandText = $"SELECT * FROM {DbTableName.Users} WHERE user = @user;";
                selectCommand.Parameters.AddWithValue("@user", user);

                var query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    return new User
                    {
                        Name= query.GetString(0),
                        Password= query.GetString(1),
                        Role= query.GetString(2)
                    };
                }
            }
            return null;
        }
        /// <summary>
        /// 插入用户到数据库中
        /// </summary>
        public static void InsertUser(User user)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = db.CreateCommand();
                insertCommand.CommandText = $"INSERT INTO {DbTableName.Users} VALUES (@User, @Password, @Role);";
                insertCommand.Parameters.AddWithValue("@User", user.Name);
                insertCommand.Parameters.AddWithValue("@Password", user.Password);
                insertCommand.Parameters.AddWithValue("@Role", user.Role);

                insertCommand.ExecuteReader();
            }
        }
    }
}