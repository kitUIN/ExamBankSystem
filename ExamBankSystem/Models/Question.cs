using CommunityToolkit.Mvvm.ComponentModel;
using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamBankSystem.Models
{
    /// <summary>
    /// 问题
    /// </summary>
    public partial class Question : ObservableObject
    {
        /// <summary>
        /// ID
        /// </summary>
        [ObservableProperty]
        private int id;
        /// <summary>
        /// 科目
        /// </summary>
        [ObservableProperty]
        private string subject;
        /// <summary>
        /// 类型
        /// </summary>
        [ObservableProperty]
        private QuestionType type;
        /// <summary>
        /// 问题
        /// </summary>
        [ObservableProperty]
        private string name;
        /// <summary>
        /// 答案
        /// </summary>
        [ObservableProperty]
        private string answer;
        /// <summary>
        /// 分值
        /// </summary>
        [ObservableProperty]
        private double point;
        /// <summary>
        /// 难易度
        /// </summary>
        [ObservableProperty]
        private int rank;
        /// <summary>
        /// 知识点名称
        /// </summary>
        [ObservableProperty]
        private string knowledgeName;        
        /// <summary>
        /// 上传用户
        /// </summary>
        [ObservableProperty]
        private string uploadUser;
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
        public static Question FromDb(SqliteDataReader query)
        {
            return new Question
            {
                Id = query.GetInt32(0),
                Subject = query.GetString(1),
                Type = (QuestionType)query.GetInt32(2),
                Name = query.GetString(3),
                Answer = query.GetString(4),
                Point = query.GetDouble(5),
                Rank = query.GetInt32(6),
                KnowledgeName = query.GetString(7),
                UploadUser = query.GetString(8),
                CreateTime = DateTimeHelper.ToDateTime(query.GetInt64(9)),
                UpdateTime = DateTimeHelper.ToDateTime(query.GetInt64(10)),
            };
        }
    }
}
