using System.Collections.Generic;
using ExamBankSystem.Enums;
using ExamBankSystem.Models;
using Microsoft.Data.Sqlite;

namespace ExamBankSystem.Helpers
{
    public partial class DbHelper
    {
        #region User
        /// <summary>
        /// 创建用户表
        /// </summary>
        private static async void CreateUsersTable(SqliteConnection db)
        {
            var tableCommand = "CREATE TABLE IF NOT EXISTS `Users` (" +
                               "`id` INT AUTO_INCREMENT  PRIMARY KEY, " +
                               "`user` NVARCHAR(10) PRIMARY KEY, " +
                               "`password` NVARCHAR(16) NOT NULL, " +
                               "`role` NVARCHAR(20) NOT NULL, " +
                               "`createTime` TIMESTAMP NOT NULL, " +
                               "`lastLoginTime` TIMESTAMP NULL " +
                               ")";
            var createTable = new SqliteCommand(tableCommand, db);
            await createTable.ExecuteReaderAsync();
        }
        /// <summary>
        /// 从数据库中获取用户
        /// </summary>
        public static List<User> GetUsers()
        {
            var res = new List<User>();
            using (var db = new SqliteConnection($"Filename={_dbpath}"))
            {
                db.Open();
                var selectCommand = db.CreateCommand();
                selectCommand.CommandText = $"SELECT * FROM {DbTableName.Users};";

                var query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    res.Add( User.FromDb(query));
                }
            }
            return res;
        }
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
    }
}