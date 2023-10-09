using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using Microsoft.Data.Sqlite;
using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ExamBankSystem.Models
{
    /// <summary>
    /// 用户
    /// </summary>
    public partial class User : OrderModel
    {
        /// <summary>
        /// ID
        /// </summary>
        [ObservableProperty]
        private int id;
        /// <summary>
        /// 名称
        /// </summary>
        [ObservableProperty]
        private string name ;
        /// <summary>
        /// 密码
        /// </summary>
        [ObservableProperty]
        private string password ;
        /// <summary>
        /// 权限
        /// </summary>
        [ObservableProperty]
        private UserRole role ;
        /// <summary>
        /// 创建时间
        /// </summary>
        [ObservableProperty]
        private DateTime createTime ;
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [ObservableProperty]
        private DateTime lastLoginTime ;
        public User() { }
        public User(SqliteDataReader query):base(query)
        {
            Id = query.GetInt32(0);
            Name = query.GetString(1);
            Password = query.GetString(2);
            Role = EnumHelper.GetEnum<UserRole>(query.GetString(3));
            CreateTime = DateTimeHelper.ToDateTime(query.GetInt64(4));
            LastLoginTime = DateTimeHelper.ToDateTime(query.GetInt64(5));

        }
    }
}
