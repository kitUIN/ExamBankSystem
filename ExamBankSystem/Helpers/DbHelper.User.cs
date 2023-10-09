using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        private static void CreateUsersTable()
        {
            ExecuteReaderAsync(command =>
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS `Users` (" +
                                      "`id` INTEGER PRIMARY KEY AUTOINCREMENT , " +
                                      "`user` NVARCHAR(10) NOT NULL, " +
                                      "`password` NVARCHAR(32) NOT NULL, " +
                                      "`role` NVARCHAR(20) NOT NULL, " +
                                      "`createTime` TIMESTAMP NOT NULL, " +
                                      "`lastLoginTime` TIMESTAMP NULL " +
                                      ")";
                return command;
            });
        }

        /// <summary>
        /// 从数据库中获取用户
        /// </summary>
        public static async Task<List<User>> GetUsersAsync(long page = 1, int limit = 15)
        {
            return await GetAsync<User>(page, limit);
        }

        /// <summary>
        /// 从数据库中获取用户
        /// </summary>
        /// <param name="user">用户名</param>
        public static async Task<User> GetUserAsync(string user)
        {
            return await ExecuteReaderAsync(selectCommand =>
            {
                selectCommand.CommandText = "SELECT * FROM `Users` WHERE `user` = @user;";
                selectCommand.Parameters.AddWithValue("@user", user);
                return selectCommand;
            }, query =>
            {
                while (query.Read())
                {
                    return new User(query);
                }

                return null;
            });
        }

        /// <summary>
        /// 从数据库中获取用户
        /// </summary>
        /// <param name="id">用户id</param>
        public static async Task<User> GetUserAsync(int id)
        {
            return await GetByIdAsync<User>(id);
        }

        /// <summary>
        /// 插入用户到数据库中
        /// </summary>
        public static void InsertUser(string name, string password, string role)
        {
            ExecuteReaderAsync(selectCommand =>
            {
                selectCommand.CommandText =
                    "INSERT INTO `Users` VALUES (NULL, @User, @Password, @Role, @CreateTime, @UpdateTime);";
                selectCommand.Parameters.AddWithValue("@User", name);
                selectCommand.Parameters.AddWithValue("@Password", password);
                selectCommand.Parameters.AddWithValue("@Role", role);
                var t = DateTimeHelper.GetTimeStamp();
                selectCommand.Parameters.AddWithValue("@CreateTime", t);
                selectCommand.Parameters.AddWithValue("@UpdateTime", t);
                return selectCommand;
            });
        }

        /// <summary>
        /// 更新用户登录时间
        /// </summary>
        public static void UpdateUserLoginTime(int id)
        {
            ExecuteReaderAsync(selectCommand =>
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
            ExecuteReaderAsync(selectCommand =>
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
        public static void ResetUserPassword(int id, string name)
        {
            UpdateUserPassword(id, HashHelper.Hash_MD5_32(name.Substring(name.Length - 3)));
        }

        /// <summary>
        /// 从数据库中删除用户
        /// </summary>
        public static void DeleteUser(int id)
        {
            DeleteById<User>(id);
        }
        /// <summary>
        /// 搜索考试科目
        /// </summary>
        public static async Task<List<User>> SearchUserAsync(string keyword, long page = 1,
            int limit = 15)
        {
            return await SearchAsync<User>("user", keyword, page, limit);
        } 
    }
}