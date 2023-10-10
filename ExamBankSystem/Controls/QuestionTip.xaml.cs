using ExamBankSystem.Args;
using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using ExamBankSystem.Models;
using ExamBankSystem.Utils;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

namespace ExamBankSystem.Controls
{
    public sealed partial class QuestionTip : UserControl
    {
        /// <summary>
        /// 请求刷新事件
        /// </summary>
        public event EventHandler RefreshEvent;
        public QuestionTip()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 待修改的问题
        /// </summary>
        private Question _question;

        private ActionMode Mode { get; set; }

        /// <summary>
        /// 显示
        /// </summary>
        public async void Show(ActionMode mode, object obj = null)
        {
            Mode = mode;
            switch (mode)
            {
                case ActionMode.Add:
                    MainTeachingTip.IsOpen = true;
                    MainTeachingTip.Title = ResourcesHelper.GetString(ResourceKey.Add) +
                                            ResourcesHelper.GetString(ResourceKey.Questions);
                    Subject.Text = "";
                    QuestionType.SelectedIndex = 0;
                    Question.Text = "";
                    Choices.Text = "";
                    Answer.Text = "";
                    Rank.SelectedIndex = 0;
                    KnowledgePoint.Text = "";
                    break;
                case ActionMode.Edit:
                    if (obj is Question question)
                    {
                        MainTeachingTip.Title = ResourcesHelper.GetString(ResourceKey.Edit) +
                                                ResourcesHelper.GetString(ResourceKey.Questions);
                        MainTeachingTip.IsOpen = true;
                        if (await DbHelper.QuestionHasAnyTestPaperAsync(question.Id))
                        {
                            EventHelper.InvokeTipPopup(this, ResourcesHelper.GetString(ResourceKey.Delete) +
                                                             ResourcesHelper.GetString(ResourceKey.QuestionFail),
                                InfoBarSeverity.Error
                            );
                        }

                        _question = question;
                    }

                    break;
                case ActionMode.Delete:
                    Delete(obj);
                    break;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="obj"></param>
        private void Delete(object obj)
        {
            if (obj is IList<object> items)
            {
                EventHelper.InvokeTopGridEvent(this,
                    new TopGridEventArg(
                        XamlHelper.CreateDeleteDialog(async (sender, args) =>
                            {
                                var success = 0;
                                var fail = 0;
                                foreach (var item in items.Cast<Question>())
                                {
                                    if (await DbHelper.QuestionHasAnyTestPaperAsync(item.Id))
                                    {
                                        fail++;
                                    }
                                    else
                                    {
                                        success++;
                                        DbHelper.DeleteById<Question>(item.Id);
                                    }
                                }

                                if (success > 0)
                                {
                                    EventHelper.InvokeTipPopup(this,
                                        ResourcesHelper.GetString(ResourceKey.DeleteSuccess) + ": " + success,
                                        InfoBarSeverity.Success
                                    );
                                }

                                if (fail > 0)
                                {
                                    EventHelper.InvokeTipPopup(this,
                                        ResourcesHelper.GetString(ResourceKey.Delete) +
                                        ResourcesHelper.GetString(ResourceKey.QuestionFail) + ": " +
                                        fail,
                                        InfoBarSeverity.Error
                                    );
                                }

                                RefreshEvent?.Invoke(this, EventArgs.Empty);
                            }
                        ),
                        TopGridMode.ContentDialog));
            }
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            MainTeachingTip.IsOpen = false;
        }

        /// <summary>
        /// 添加或修改按钮响应
        /// </summary>
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // 检测是否为空

            #region NotNull

            var subjectId = -1;
            if (await DbHelper.GetExamSubjectAsync(Subject.Text) is ExamSubject subject)
            {
                subjectId = subject.Id;
            }
            else
            {
                EventHelper.InvokeTipPopup(this,
                    ResourcesHelper.GetString(ResourceKey.ExamSubjects) +
                    ResourcesHelper.GetString(ResourceKey.NotExist),
                    InfoBarSeverity.Error
                );
                return;
            }

            if (string.IsNullOrEmpty(Subject.Text))
            {
                EventHelper.InvokeTipPopup(this,
                    ResourcesHelper.GetString(ResourceKey.ExamSubjectNull),
                    InfoBarSeverity.Error
                );
                return;
            }

            if (string.IsNullOrEmpty(Question.Text))
            {
                EventHelper.InvokeTipPopup(this,
                    ResourcesHelper.GetString(ResourceKey.Questions) +
                    ResourcesHelper.GetString(ResourceKey.NotNull),
                    InfoBarSeverity.Error
                );
                return;
            }
             
            if (string.IsNullOrEmpty(Answer.Text))
            {
                EventHelper.InvokeTipPopup(this,
                    ResourcesHelper.GetString(ResourceKey.Answer) +
                    ResourcesHelper.GetString(ResourceKey.NotNull),
                    InfoBarSeverity.Error
                );
                return;
            }

            double point = 0;
            try
            {
                point = Convert.ToDouble(Choices.Text);
            }
            catch (Exception)
            {
                EventHelper.InvokeTipPopup(this,
                    ResourcesHelper.GetString(ResourceKey.Point) +
                    ResourcesHelper.GetString(ResourceKey.NotNull),
                    InfoBarSeverity.Error
                );
                return;
            }

            var knowledgeId = -1;
            if (await DbHelper.GetKnowledgePointAsync(KnowledgePoint.Text) is KnowledgePoint kpoint)
            {
                knowledgeId = kpoint.Id;
            }
            else
            {
                EventHelper.InvokeTipPopup(this,
                    ResourcesHelper.GetString(ResourceKey.KnowledgePoints) +
                    ResourcesHelper.GetString(ResourceKey.NotExist),
                    InfoBarSeverity.Error
                );
                return;
            }

            #endregion

            switch (Mode)
            {
                case ActionMode.Add:
                    if (await DbHelper.CheckQuestionPercent(Question.Text, QuestionType.SelectedIndex))
                    {
                        EventHelper.InvokeTipPopup(this,
                            ResourcesHelper.GetString(ResourceKey.QuestionPercentError),
                            InfoBarSeverity.Error
                        );
                    }
                    else
                    {
                        DbHelper.InsertQuestion(
                            subjectId, QuestionType.SelectedIndex, Question.Text,
                            point, Answer.Text, Rank.SelectedIndex + 1, knowledgeId, CurrentData.CurrentUser.Id
                        );
                        EventHelper.InvokeTipPopup(this,
                            ResourcesHelper.GetString(ResourceKey.AddSuccess),
                            InfoBarSeverity.Success
                        );
                    }
                    break;
                case ActionMode.AddMul:
                    break;
                case ActionMode.Edit:
                    if (await DbHelper.CheckQuestionPercent(Question.Text, QuestionType.SelectedIndex))
                    {
                        EventHelper.InvokeTipPopup(this,
                            ResourcesHelper.GetString(ResourceKey.QuestionPercentError),
                            InfoBarSeverity.Error
                        );
                    }
                    else
                    {
                        DbHelper.UpdateQuestion(_question.Id, subjectId, QuestionType.SelectedIndex, Question.Text,
                        point, Answer.Text, Rank.SelectedIndex + 1, knowledgeId, CurrentData.CurrentUser.Id
                    );
                        EventHelper.InvokeTipPopup(this,
                            ResourcesHelper.GetString(ResourceKey.EditSuccess),
                            InfoBarSeverity.Success
                        );
                    }
                    break;
            }

            Hide();
            RefreshEvent?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 回车响应
        /// </summary>
        private void Button_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                AddButton_Click(sender, e);
            }
        }
    }
}