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
        [ObservableProperty]
        private string name;
        [ObservableProperty]
        private string knowledge;
        [ObservableProperty]
        private DateTime createTime;
        [ObservableProperty]
        private DateTime updateTime;
        public static KnowledgePoint FromDb(SqliteDataReader query)
        {
            return new KnowledgePoint
            {
                Name = query.GetString(0),
                Knowledge = query.GetString(1),
                CreateTime = DateTimeHelper.ToDateTime(query.GetInt64(2)),
                UpdateTime = DateTimeHelper.ToDateTime(query.GetInt64(3)),
            };
        }
    }
}
