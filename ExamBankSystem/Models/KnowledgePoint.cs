using CommunityToolkit.Mvvm.ComponentModel;
using ExamBankSystem.Helpers;
using System;
using System.Data;

namespace ExamBankSystem.Models
{
    public partial class KnowledgePoint: OrderModel
    {
        /// <summary>
        /// ID
        /// </summary>
        [ObservableProperty]
        private int id;
        /// <summary>
        /// 考试科目ID
        /// </summary>
        [ObservableProperty]
        private int subjectId;
        /// <summary>
        /// 考试科目
        /// </summary>
        [ObservableProperty]
        private string subject;
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
        public KnowledgePoint(){ }
        public KnowledgePoint(IDataRecord query):base(query)
        {
            Id = query.GetInt32(0);
            SubjectId = query.GetInt32(1);
            Name = query.GetString(2);
            Knowledge = query.GetString(3);
            CreateTime = DateTimeHelper.ToDateTime(query.GetInt64(4));
            UpdateTime = DateTimeHelper.ToDateTime(query.GetInt64(5));
        }
        
        partial void OnSubjectIdChanged(int oldValue, int newValue)
        {
            if (oldValue == newValue) return;
            if (DbHelper.GetExamSubject(newValue) is { } examSubject)
            {
                Subject = examSubject.Name;
            }
        }
    }
}
