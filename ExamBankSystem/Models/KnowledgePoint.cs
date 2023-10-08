using CommunityToolkit.Mvvm.ComponentModel;
using ExamBankSystem.Helpers;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamBankSystem.Models
{
    public partial class KnowledgePoint: ObservableObject
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
        /// 知识点
        /// </summary>
        [ObservableProperty]
        private string knowledge;
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
        public static KnowledgePoint FromDb(SqliteDataReader query)
        {
            return new KnowledgePoint
            {
                Id = query.GetInt32(0),
                Name = query.GetString(1),
                Knowledge = query.GetString(2),
                CreateTime = DateTimeHelper.ToDateTime(query.GetInt64(3)),
                UpdateTime = DateTimeHelper.ToDateTime(query.GetInt64(4)),
            };
        }
    }
}
