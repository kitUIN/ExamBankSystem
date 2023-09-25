using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamBankSystem.Models
{
    /// <summary>
    /// 试卷内题目
    /// </summary>
    public partial class QuestionPaper : ObservableObject
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
    }
}
