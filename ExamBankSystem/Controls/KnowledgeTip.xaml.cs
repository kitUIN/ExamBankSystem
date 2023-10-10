using ExamBankSystem.Args;
using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using ExamBankSystem.Models;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml;

namespace ExamBankSystem.Controls
{
    public sealed partial class KnowledgeTip : UserControl
    {
        public KnowledgeTip()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// 待修改的知识点
        /// </summary>
        private KnowledgePoint _knowledge;

        private ActionMode Mode { get; set; }

        /// <summary>
        /// 请求刷新事件
        /// </summary>
        public event EventHandler RefreshEvent;

        /// <summary>
        /// 显示
        /// </summary>
        public void Show(ActionMode mode, object obj = null)
        {
            Mode = mode;
            switch (mode)
            {
                case ActionMode.Add:
                    MainTeachingTip.IsOpen = true;
                    MainTeachingTip.Title = ResourcesHelper.GetString(ResourceKey.Add) +
                                            ResourcesHelper.GetString(ResourceKey.KnowledgePoints);
                    Subject.Text = "";
                    Knowledge.Text = "";
                    KnowledgeName.Text = "";
                    break;
                case ActionMode.Edit:
                    if (obj is KnowledgePoint point)
                    {
                        MainTeachingTip.Title = ResourcesHelper.GetString(ResourceKey.Edit) +
                                                ResourcesHelper.GetString(ResourceKey.KnowledgePoints);
                        MainTeachingTip.IsOpen = true;
                        _knowledge = point;
                        Subject.Text = point.Subject;
                        Knowledge.Text = point.Knowledge;
                        KnowledgeName.Text = point.Name;
                    }
                    break;
                case ActionMode.Delete:
                    if (obj is IList<object> items)
                    {
                        EventHelper.InvokeTopGridEvent(this,
                            new TopGridEventArg(
                                XamlHelper.CreateDeleteDialog(async (sender, args) =>
                                {
                                    var success = 0;
                                    var fail = 0;
                                    foreach (var item in items.Cast<KnowledgePoint>())
                                    {
                                        if (await DbHelper.KnowledgeHasAnyQuestionAsync(item.Id))
                                        {
                                            fail++;
                                        }
                                        else
                                        {
                                            success++;
                                            DbHelper.DeleteById<KnowledgePoint>(item.Id);
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
                                            ResourcesHelper.GetString(ResourceKey.KnowledgePointDeleteFail) + ": " +
                                            fail,
                                            InfoBarSeverity.Error
                                        );
                                    }
                                    RefreshEvent?.Invoke(this, EventArgs.Empty);
                                }
                                ),
                                TopGridMode.ContentDialog));
                    }

                    break;
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
            if (string.IsNullOrEmpty(Subject.Text))
            {
                EventHelper.InvokeTipPopup(this,
                    ResourcesHelper.GetString(ResourceKey.ExamSubjectNull),
                    InfoBarSeverity.Error
                );
                return;
            }
            if (string.IsNullOrEmpty(KnowledgeName.Text))
            {
                EventHelper.InvokeTipPopup(this,
                    ResourcesHelper.GetString(ResourceKey.KnowledgePoints) +
                    ResourcesHelper.GetString(ResourceKey.NotNull),
                    InfoBarSeverity.Error
                );
                return;
            }
            if (string.IsNullOrEmpty(Knowledge.Text))
            {
                EventHelper.InvokeTipPopup(this,
                    ResourcesHelper.GetString(ResourceKey.KnowledgePointContent) +
                    ResourcesHelper.GetString(ResourceKey.NotNull),
                    InfoBarSeverity.Error
                );
                return;
            }
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
            switch (Mode)
            {
                case ActionMode.Add:
                    if (await DbHelper.GetKnowledgePointAsync(KnowledgeName.Text) == null)
                    {
                        DbHelper.InsertKnowledgePoint(KnowledgeName.Text,Knowledge.Text,subjectId);
                        EventHelper.InvokeTipPopup(this,
                            ResourcesHelper.GetString(ResourceKey.KnowledgePoints)+
                            ResourcesHelper.GetString(ResourceKey.AddSuccess),
                            InfoBarSeverity.Success
                        );
                    }
                    else
                    {
                        EventHelper.InvokeTipPopup(this,
                            ResourcesHelper.GetString(ResourceKey.KnowledgePoints) +
                            ResourcesHelper.GetString(ResourceKey.Exist),
                            InfoBarSeverity.Error
                        );
                        return;
                    }

                    break;
                case ActionMode.Edit:
                    if (_knowledge.Name == KnowledgeName.Text || await DbHelper.GetKnowledgePointAsync(KnowledgeName.Text) == null)
                    {
                        DbHelper.UpdateKnowledgePoint(_knowledge.Id, KnowledgeName.Text, Knowledge.Text, subjectId);
                        EventHelper.InvokeTipPopup(this,
                            ResourcesHelper.GetString(ResourceKey.EditSuccess),
                            InfoBarSeverity.Success
                        );
                    }
                    else
                    {
                        EventHelper.InvokeTipPopup(this,
                            ResourcesHelper.GetString(ResourceKey.KnowledgePoints) +
                            ResourcesHelper.GetString(ResourceKey.Exist),
                            InfoBarSeverity.Error
                        );
                        return;
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
