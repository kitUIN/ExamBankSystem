using CommunityToolkit.Mvvm.ComponentModel;
using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ExamBankSystem.Models
{
    /// <summary>
    /// 考试科目
    /// </summary>
    public partial class ExamSubject: ObservableObject
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
        private string name;
        /// <summary>
        /// 创建时间
        /// </summary>
        [ObservableProperty]
        private DateTime createTime;
        /// <summary>
        /// 更新时间
        /// </summary>
        [ObservableProperty]
        private DateTime updateTime;
        /// <summary>
        /// 从数据库导入
        /// </summary>
        public static ExamSubject FromDb(SqliteDataReader query)
        {
            return new ExamSubject
            {
                Id = query.GetInt32(0),
                Name = query.GetString(1),
                CreateTime = DateTimeHelper.ToDateTime(query.GetInt64(2)),
                UpdateTime = DateTimeHelper.ToDateTime(query.GetInt64(3)),
            };
        }
    }
}
