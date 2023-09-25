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

        #region User
        /// <summary>
        /// 从数据库中获取用户
        /// </summary>
        /// <param name="user">用户名</param>
        public static User GetUser(string user)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();
                var selectCommand = db.CreateCommand();
                selectCommand.CommandText = $"SELECT * FROM {DbTableName.Users} WHERE user = @user;";
                selectCommand.Parameters.AddWithValue("@user", user);

                var query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    return User.FromDb(query);
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

                var insertCommand = db.CreateCommand();
                insertCommand.CommandText = $"INSERT INTO {DbTableName.Users} VALUES (@User, @Password, @Role);";
                insertCommand.Parameters.AddWithValue("@User", user.Name);
                insertCommand.Parameters.AddWithValue("@Password", user.Password);
                insertCommand.Parameters.AddWithValue("@Role", user.Role);

                insertCommand.ExecuteReader();
            }
        }
        /// <summary>
        /// 更新用户登录时间
        /// </summary>
        public static void UpdateUserLoginTime(string userName)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();

                var insertCommand = db.CreateCommand();
                insertCommand.CommandText = $"UPDATE {DbTableName.Users} SET lastLoginTime = @loginTime WHERE user = @user;";
                insertCommand.Parameters.AddWithValue("@loginTime", DateTimeHelper.GetTimeStamp());
                insertCommand.Parameters.AddWithValue("@user", userName);
                insertCommand.ExecuteReader();
            }
        }
        #endregion

        #region ExamSubject
        /// <summary>
        /// 从数据库中获取考试科目
        /// </summary>
        public static List<ExamSubject> GetExamSubjects()
        {
            List<ExamSubject> res = new List<ExamSubject>(); 
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

        #region Question
        /// <summary>
        /// 从数据库中获取题目
        /// </summary>
        public static List<Question> GetQuestions()
        {
            var res = new List<Question>();
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();
                var selectCommand = db.CreateCommand();
                selectCommand.CommandText = $"SELECT * FROM {DbTableName.Questions} ;";
                var query = selectCommand.ExecuteReader();
                while (query.Read())
                {
                    res.Add(Question.FromDb(query));
                }
            }
            return res;
        }
        /// <summary>
        /// 从数据库中获取知识点
        /// </summary>
        public static Question GetQuestion(int id)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();
                var selectCommand = db.CreateCommand();
                selectCommand.CommandText = $"SELECT * FROM {DbTableName.Questions} WHERE id = @Name;";

                selectCommand.Parameters.AddWithValue("@Name", id);

                var query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    return Question.FromDb(query);
                }
            }
            return null;
        }
        /// <summary>
        /// 插入考试科目到数据库中
        /// </summary>
        public static void InsertQuestion(Question instance)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();

                var insertCommand = db.CreateCommand();
                insertCommand.CommandText = $"INSERT INTO {DbTableName.Questions} (" +
                    $"subject, type, question, answer, point, rank, knowledgeName, " +
                    $"uploadUser, createTime, updateTime) " +
                    $"VALUES (@Subject, @Type, @Question, @Answer, " +
                    $"@Point, @Rank, @KnowledgeName, @UploadUser, @CreateTime, @UpdateTime);";
                insertCommand.Parameters.AddWithValue("@Subject", instance.Subject);
                insertCommand.Parameters.AddWithValue("@Type", instance.Type);
                insertCommand.Parameters.AddWithValue("@Question", instance.Name);
                insertCommand.Parameters.AddWithValue("@Answer", instance.Answer);
                insertCommand.Parameters.AddWithValue("@Point", instance.Point);
                insertCommand.Parameters.AddWithValue("@Rank", instance.Rank);
                insertCommand.Parameters.AddWithValue("@KnowledgeName", instance.KnowledgeName);
                insertCommand.Parameters.AddWithValue("@UploadUser", instance.UploadUser);
                var t = DateTimeHelper.GetTimeStamp();
                insertCommand.Parameters.AddWithValue("@CreateTime", t);
                insertCommand.Parameters.AddWithValue("@UpdateTime", t);
                insertCommand.ExecuteReader();
            }
        }

        /// <summary>
        /// 从数据库中删除知识点
        /// </summary>
        public static void DeleteQuestion(int key)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();

                var command = db.CreateCommand();
                command.CommandText = $"DELETE FROM {DbTableName.Questions} WHERE id = @Name;";
                command.Parameters.AddWithValue("@Name", key);
                command.ExecuteReader();
            }
        }
        /// <summary>
        /// 更新科目
        /// </summary>
        public static void UpdateQuestion(Question obj, int key)
        {
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();

                var insertCommand = db.CreateCommand();
                insertCommand.CommandText = $"UPDATE {DbTableName.Questions} SET question = @Question, " +
                    $"answer = @Answer, updateTime = @UpdateTime " +
                    $"subject = @Subject, type = @Type " +
                    $"point = @Point, rank = @Rank " +
                    $"knowledgeName = @KnowledgeName " +
                    $"WHERE id = @OldName;";
                insertCommand.Parameters.AddWithValue("@Subject", obj.Subject);
                insertCommand.Parameters.AddWithValue("@Type", (int)obj.Type);
                insertCommand.Parameters.AddWithValue("@Question", obj.Name);
                insertCommand.Parameters.AddWithValue("@Answer", obj.Answer);
                insertCommand.Parameters.AddWithValue("@Point", obj.Point);
                insertCommand.Parameters.AddWithValue("@Rank", obj.Rank);
                insertCommand.Parameters.AddWithValue("@KnowledgeName", obj.KnowledgeName);
                obj.UpdateTime = DateTime.Now;
                insertCommand.Parameters.AddWithValue("@UpdateTime", DateTimeHelper.ToTimeStamp(obj.UpdateTime));
                insertCommand.Parameters.AddWithValue("@OldName", key);
                insertCommand.ExecuteReader();
            }
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