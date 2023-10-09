using CommunityToolkit.Mvvm.ComponentModel;
using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using System;
using System.Data;

namespace ExamBankSystem.Models
{
    /// <summary>
    /// 问题
    /// </summary>
    public partial class Question : OrderModel
    {
        /// <summary>
        /// ID
        /// </summary>
        [ObservableProperty] private int id;

        /// <summary>
        /// 科目Id
        /// </summary>
        [ObservableProperty] private int subjectId;
        /// <summary>
        /// 科目
        /// </summary>
        [ObservableProperty]
        private string subject = "";

        /// <summary>
        /// 类型
        /// </summary>
        [ObservableProperty] private QuestionType type;

        /// <summary>
        /// 问题
        /// </summary>
        [ObservableProperty] private string name;

        /// <summary>
        /// 答案
        /// </summary>
        [ObservableProperty] private string answer;

        /// <summary>
        /// 分值
        /// </summary>
        [ObservableProperty] private double point;

        /// <summary>
        /// 难易度
        /// </summary>
        [ObservableProperty] private int rank;

        /// <summary>
        /// 知识点名称
        /// </summary>
        [ObservableProperty] private int knowledgeId;

        /// <summary>
        /// 知识点名称
        /// </summary>
        [ObservableProperty]
        public string knowledge = "";
        /// <summary>
        /// 上传用户Id
        /// </summary>
        [ObservableProperty] private int uploadUserId;

        /// <summary>
        /// 创建时间
        /// </summary>
        [ObservableProperty] private DateTime createTime;

        /// <summary>
        /// 更新时间
        /// </summary>
        [ObservableProperty] private DateTime updateTime;
        public Question(){ }
        public Question(IDataRecord query):base(query)
        {
            Id = query.GetInt32(0);
            SubjectId = query.GetInt32(1);
            Type = (QuestionType)query.GetInt32(2);
            Name = query.GetString(3);
            Point = query.GetDouble(4);
            Answer = query.GetString(5);
            Rank = query.GetInt32(6);
            KnowledgeId = query.GetInt32(7);
            UploadUserId = query.GetInt32(8);
            CreateTime = DateTimeHelper.ToDateTime(query.GetInt64(9));
            UpdateTime = DateTimeHelper.ToDateTime(query.GetInt64(10));
        }

        public string TypeToString(QuestionType ty)
        {
            return ResourcesHelper.GetString(EnumHelper.GetEnum<ResourceKey>(ty.ToString()));
        }

        partial void OnSubjectIdChanged(int oldValue, int newValue)
        {
            if (oldValue == newValue) return;
            if (DbHelper.GetExamSubject(newValue) is { } examSubject)
            {
                Subject = examSubject.Name;
            }
        }
        partial void OnKnowledgeIdChanged(int oldValue, int newValue)
        {
            if (oldValue == newValue) return;
            if (DbHelper.GetById<KnowledgePoint>(newValue)is { } knowledgePoint)
            {
                Knowledge = knowledgePoint.Name;
            }
        }
    }
}