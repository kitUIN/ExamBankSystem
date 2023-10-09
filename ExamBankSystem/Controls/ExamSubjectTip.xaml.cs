using ExamBankSystem.Args;
using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using ExamBankSystem.Models;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace ExamBankSystem.Controls
{
    public sealed partial class ExamSubjectTip : UserControl
    {
        /// <summary>
        /// 待修改的考试科目
        /// </summary>
        private ExamSubject _subject;

        private ActionMode Mode { get; set; }

        /// <summary>
        /// 请求刷新事件
        /// </summary>
        public event EventHandler RefreshEvent;

        public ExamSubjectTip()
        {
            this.InitializeComponent();
        }

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
                                            ResourcesHelper.GetString(ResourceKey.ExamSubjects);
                    ExamSubject.Text = "";
                    break;
                case ActionMode.Edit:
                    if (obj is ExamSubject subject)
                    {
                        MainTeachingTip.Title = ResourcesHelper.GetString(ResourceKey.Edit) +
                                                ResourcesHelper.GetString(ResourceKey.ExamSubjects);
                        MainTeachingTip.IsOpen = true;
                        _subject = subject;
                        ExamSubject.Text = subject.Name;
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
                                        foreach (var item in items.Cast<ExamSubject>())
                                        {
                                            if (await DbHelper.ExamSubjectHasAnyQuestionAsync(item.Id))
                                            {
                                                fail++;
                                            }
                                            else
                                            {
                                                success++;
                                                DbHelper.DeleteExamSubject(item.Id);
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
                                                ResourcesHelper.GetString(ResourceKey.ExamSubjectsDeleteFail) + ": " +
                                                fail,
                                                InfoBarSeverity.Success
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
            if (string.IsNullOrEmpty(ExamSubject.Text))
            {
                EventHelper.InvokeTipPopup(this,
                    ResourcesHelper.GetString(ResourceKey.ExamSubjectNull),
                    InfoBarSeverity.Error
                );
                return;
            }
            switch (Mode)
            {
                case ActionMode.Add:
                    if (await DbHelper.GetExamSubjectAsync(ExamSubject.Text) == null)
                    {
                        DbHelper.InsertExamSubject(ExamSubject.Text);
                        EventHelper.InvokeTipPopup(this,
                            ResourcesHelper.GetString(ResourceKey.AddSuccess),
                            InfoBarSeverity.Success
                        );
                    }
                    else
                    {
                        EventHelper.InvokeTipPopup(this,
                            ResourcesHelper.GetString(ResourceKey.ExamSubjects) +
                            ResourcesHelper.GetString(ResourceKey.Exist),
                            InfoBarSeverity.Error
                        );
                        return;
                    }

                    break;
                case ActionMode.Edit:
                    DbHelper.UpdateExamSubjectName(_subject.Id, ExamSubject.Text);
                    EventHelper.InvokeTipPopup(this,
                        ResourcesHelper.GetString(ResourceKey.EditSuccess),
                        InfoBarSeverity.Success
                    );
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
            if(e.Key == Windows.System.VirtualKey.Enter)
            {
                AddButton_Click(sender, e);
            }
        }
    }
}