using CommunityToolkit.Mvvm.ComponentModel;
using ExamBankSystem.Helpers;
using System;
using System.Data;

namespace ExamBankSystem.Models
{
    /// <summary>
    /// 考试科目
    /// </summary>
    public partial class ExamSubject : OrderModel
    {
        /// <summary>
        /// ID
        /// </summary>
        [ObservableProperty] private int id;

        /// <summary>
        /// 名称
        /// </summary>
        [ObservableProperty] private string name;

        /// <summary>
        /// 创建时间
        /// </summary>
        [ObservableProperty] private DateTime createTime;

        /// <summary>
        /// 更新时间
        /// </summary>
        [ObservableProperty] private DateTime updateTime;

        /// <summary>
        /// 从数据库导入
        /// </summary>
        public ExamSubject(IDataRecord query): base(query)
        {
            Id = query.GetInt32(0);
            Name = query.GetString(1);
            CreateTime = DateTimeHelper.ToDateTime(query.GetInt64(2));
            UpdateTime = DateTimeHelper.ToDateTime(query.GetInt64(3));
        }
    }
}