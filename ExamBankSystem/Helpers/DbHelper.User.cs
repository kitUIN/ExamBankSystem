using System.Collections.Generic;
using System.Linq;
using ExamBankSystem.Enums;
using ExamBankSystem.Models;
using Microsoft.Data.Sqlite;

namespace ExamBankSystem.Helpers
{
    public partial class DbHelper
    {
        /// <summary>
        /// 创建用户表
        /// </summary>
        private static async void CreateUsersTable(SqliteConnection db)
        {
            var tableCommand = "CREATE TABLE IF NOT EXISTS `Users` (" +
                               "`id` INTEGER PRIMARY KEY AUTOINCREMENT , " +
                               "`user` NVARCHAR(10) NOT NULL, " +
                               "`password` NVARCHAR(32) NOT NULL, " +
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
        public static List<User> GetUsers(int page = 0,int limit = 20)
        {
            return Execute(selectCommand =>
            {
                selectCommand.CommandText = "SELECT * FROM `Users` LIMIT @Offset OFFSET @Page ;";
                selectCommand.Parameters.AddWithValue("@Offset", limit);
                selectCommand.Parameters.AddWithValue("@Page", page);
                return selectCommand;
            }, query =>
            {
                var res = new List<User>();
                while (query.Read())
                {
                    res.Add(User.FromDb(query));
                }
                return res;
            });
        }
        /// <summary>
        /// 从数据库中获取用户
        /// </summary>
        /// <param name="user">用户名</param>
        public static User GetUser(string user)
        {
            return Execute(selectCommand =>
            {
                selectCommand.CommandText = "SELECT * FROM `Users` WHERE `user` = @user;";
                selectCommand.Parameters.AddWithValue("@user", user);
                return selectCommand;
            }, query =>
            {
                while (query.Read())
                {
                    return User.FromDb(query);
                }
                return null;
            });
        }
        
        /// <summary>
        /// 从数据库中获取用户
        /// </summary>
        /// <param name="id">用户id</param>
        public static User GetUser(int id)
        {
            return Execute(selectCommand =>
            {
                selectCommand.CommandText = "SELECT * FROM `Users` WHERE `id` = @ID;";
                selectCommand.Parameters.AddWithValue("@ID", id);
                return selectCommand;
            }, query =>
            {
                while (query.Read())
                {
                    return User.FromDb(query);
                }
                return null;
            });
        }
        /// <summary>
        /// 插入用户到数据库中
        /// </summary>
        public static void InsertUser(string name,string password,string role)
        {
            Execute(selectCommand =>
            {
                selectCommand.CommandText = "INSERT INTO `Users` VALUES (NULL, @User, @Password, @Role, @CreateTime, @UpdateTime);";
                selectCommand.Parameters.AddWithValue("@User", name);
                selectCommand.Parameters.AddWithValue("@Password", password);
                selectCommand.Parameters.AddWithValue("@Role", role);
                var t = DateTimeHelper.GetTimeStamp();
                selectCommand.Parameters.AddWithValue("@CreateTime", t);
                selectCommand.Parameters.AddWithValue("@UpdateTime",t);
                return selectCommand;
            });
        }
        /// <summary>
        /// 更新用户登录时间
        /// </summary>
        public static void UpdateUserLoginTime(int id)
        {
            Execute(selectCommand =>
            {
                selectCommand.CommandText = "UPDATE `Users` SET `lastLoginTime` = @loginTime WHERE `id` = @ID;";
                selectCommand.Parameters.AddWithValue("@loginTime", DateTimeHelper.GetTimeStamp());
                selectCommand.Parameters.AddWithValue("@ID", id);
                return selectCommand;
            });
        }
        /// <summary>
        /// 从数据库中修改用户密码
        /// </summary>
        public static void UpdateUserPassword(int id, string newPassword)
        {
            Execute(selectCommand =>
            {
                selectCommand.CommandText = "UPDATE `Users` SET `password` = @Password WHERE `id` = @ID;";
                selectCommand.Parameters.AddWithValue("@Password", newPassword);
                selectCommand.Parameters.AddWithValue("@ID", id);
                return selectCommand;
            });
        }
        /// <summary>
        /// 从数据库中重置用户密码
        /// </summary>
        public static void ResetUserPassword(int id)
        {
            UpdateUserPassword(id,"9b58b783a23eb7dd11f26e0a46e11ea8"); // qust123
        }
        
        /// <summary>
        /// 从数据库中删除用户
        /// </summary>
        public static void DeleteUser(int id)
        {
            Execute(selectCommand =>
            {
                selectCommand.CommandText = "DELETE FROM `Users` WHERE `id` = @ID;";
                selectCommand.Parameters.AddWithValue("@ID", id);
                return selectCommand;
            });
        }
    }
}