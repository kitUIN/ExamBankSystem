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
    public partial class DbHelper
    {
        private static string _dbpath;
        private static T Execute<T>(Func<SqliteCommand,SqliteCommand> command, Func<SqliteDataReader,T> res)
        {
            using var db = new SqliteConnection($"Filename={_dbpath}");
            db.Open();
            var dbCommand = command(db.CreateCommand());
            return res(dbCommand.ExecuteReader());
        }
        private static void Execute(Func<SqliteCommand,SqliteCommand> command)
        {
            using var db = new SqliteConnection($"Filename={_dbpath}");
            db.Open();
            var dbCommand = command(db.CreateCommand());
            dbCommand.ExecuteReader();
        }
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
        /// 创建试卷表
        /// </summary>
        private static async void CreateTestPapersTable(SqliteConnection db)
        {
            var tableCommand = "CREATE TABLE IF NOT EXISTS `TestPapers` (" +
                               "`id` INTEGER PRIMARY KEY AUTOINCREMENT , " +
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
                               "`id` INTEGER PRIMARY KEY AUTOINCREMENT , " +
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

        public static void InsertQuestionPaper(QuestionPaper instance)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();

                var insertCommand = db.CreateCommand();
                insertCommand.CommandText = $"INSERT INTO {DbTableName.QuestionPapers} (testPaperId, questionIndex, questionId, uploadUser, createTime, updateTime) VALUES (@TestPaperId, @QuestionIndex, @QuestionId, @UploadUser, @CreateTime, @UpdateTime);";
                insertCommand.Parameters.AddWithValue("@TestPaperId", instance.TestPaperId);
                insertCommand.Parameters.AddWithValue("@QuestionIndex", instance.QuestionIndex);
                insertCommand.Parameters.AddWithValue("@QuestionId", instance.QuestionId);
                insertCommand.Parameters.AddWithValue("@UploadUser", instance.UploadUser);
                var t = DateTimeHelper.GetTimeStamp();
                insertCommand.Parameters.AddWithValue("@CreateTime", t);
                insertCommand.Parameters.AddWithValue("@UpdateTime", t);
                insertCommand.ExecuteReader();
            }
        }
        #region TestPaper
        /// <summary>
        /// 从数据库中获取试卷
        /// </summary>
        public static List<TestPaper> GetTestPapers()
        {
            var res = new List<TestPaper>();
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();
                var selectCommand = db.CreateCommand();
                selectCommand.CommandText = $"SELECT * FROM {DbTableName.TestPapers} ;";
                var query = selectCommand.ExecuteReader();
                while (query.Read())
                {
                    res.Add(TestPaper.FromDb(query));
                }
            }
            return res;
        }
        /// <summary>
        /// 从数据库中获取试卷
        /// </summary>
        public static TestPaper GetTestPaper(int id)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();
                var selectCommand = db.CreateCommand();
                selectCommand.CommandText = $"SELECT * FROM {DbTableName.TestPapers} WHERE id = @Name;";

                selectCommand.Parameters.AddWithValue("@Name", id);

                var query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    return TestPaper.FromDb(query);
                }
            }
            return null;
        }
        public static void InsertTestPaper(TestPaper instance)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();

                var insertCommand = db.CreateCommand();
                insertCommand.CommandText = $"INSERT INTO {DbTableName.TestPapers} (name, point, isExamine, uploadUser, createTime, updateTime) VALUES (@Name, @Point, @IsExamine, @UploadUser, @CreateTime, @UpdateTime);";
                insertCommand.Parameters.AddWithValue("@Name", instance.Name);
                insertCommand.Parameters.AddWithValue("@Point", instance.Point);
                insertCommand.Parameters.AddWithValue("@IsExamine", instance.IsExamine);
                insertCommand.Parameters.AddWithValue("@UploadUser", instance.UploadUser);
                var t = DateTimeHelper.GetTimeStamp();
                insertCommand.Parameters.AddWithValue("@CreateTime", t);
                insertCommand.Parameters.AddWithValue("@UpdateTime", t);
                insertCommand.ExecuteReader();
            }
        }

        #endregion
    }
}