using CommunityToolkit.Mvvm.ComponentModel;
using ExamBankSystem.Controls;
using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExamBankSystem.Models
{
    /// <summary>
    /// 试卷内题目
    /// </summary>
    public partial class QuestionPaper : OrderModel
    {
        /// <summary>
        /// ID
        /// </summary>
        [ObservableProperty]
        private int id;
        /// <summary>
        /// 试卷ID
        /// </summary>
        [ObservableProperty]
        private int testPaperId;
        /// <summary>
        /// 问题序号
        /// </summary>
        [ObservableProperty]
        private int questionIndex;
        /// <summary>
        /// 问题ID
        /// </summary>
        [ObservableProperty]
        private int questionId;
        /// <summary>
        /// 问题
        /// </summary>
        [ObservableProperty]
        private Question question;
        /// <summary>
        /// 上传用户
        /// </summary>
        [ObservableProperty]
        private int uploadUserId;
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
        public QuestionPaper() { }
        public QuestionPaper(IDataRecord query) : base(query)
        {
            Id = query.GetInt32(0);
            TestPaperId = query.GetInt32(1);
            QuestionIndex =  query.GetInt32(2);
            QuestionId = query.GetInt32(3);
            UploadUserId = query.GetInt32(4);
            CreateTime = DateTimeHelper.ToDateTime(query.GetInt64(5));
            UpdateTime = DateTimeHelper.ToDateTime(query.GetInt64(6));
        }
    }
}
