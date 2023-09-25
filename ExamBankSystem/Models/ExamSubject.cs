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
    public partial class ExamSubject: ObservableObject
    {
        [ObservableProperty]
        private string name;
        [ObservableProperty]
        private DateTime createTime;
        [ObservableProperty]
        private DateTime updateTime;
        public static ExamSubject FromDb(SqliteDataReader query)
        {
            return new ExamSubject
            {
                Name = query.GetString(0),
                CreateTime = DateTimeHelper.ToDateTime(query.GetInt64(1)),
                UpdateTime = DateTimeHelper.ToDateTime(query.GetInt64(2)),
            };
        }
    }
}
