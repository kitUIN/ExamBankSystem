using CommunityToolkit.Mvvm.ComponentModel;
using ExamBankSystem.Helpers;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamBankSystem.Models
{
    /// <summary>
    /// 试卷
    /// </summary>
    public partial class TestPaper : OrderModel
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
        /// 分数
        /// </summary>
        [ObservableProperty]
        private int point;
        /// <summary>
        /// 是否审查
        /// </summary>
        [ObservableProperty]
        private bool isExamine;
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

        public TestPaper(){ }
        public TestPaper(IDataRecord query):base(query)
        {
            Id = query.GetInt32(0);
            Name = query.GetString(1);
            Point = query.GetInt32(2);
            IsExamine = query.GetBoolean(3);
            UploadUser = query.GetString(4);
            CreateTime = DateTimeHelper.ToDateTime(query.GetInt64(5));
            UpdateTime = DateTimeHelper.ToDateTime(query.GetInt64(6));
        }
    }

}
