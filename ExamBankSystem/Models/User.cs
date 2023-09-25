using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using Microsoft.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamBankSystem.Models
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastLoginTime { get; set; }
        public static User FromDb(SqliteDataReader query)
        {
            return new User
            {
                Name = query.GetString(0),
                Password = query.GetString(1),
                Role = EnumHelper.GetEnum<UserRole>(query.GetString(2)),
                CreateTime = DateTimeHelper.ToDateTime(query.GetInt64(3)),
                LastLoginTime = DateTimeHelper.ToDateTime(query.GetInt64(4)),
            };
        }
    }
}
