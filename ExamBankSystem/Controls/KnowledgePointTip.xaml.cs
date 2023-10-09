using ExamBankSystem.Args;
using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using ExamBankSystem.Models;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ExamBankSystem.Controls
{
    public sealed partial class KnowledgePointTip : UserControl
    {
        private ActionMode Mode { get; set; }

        /// <summary>
        /// 请求刷新事件
        /// </summary>
        public event EventHandler RefreshEvent;
        public KnowledgePointTip()
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
                    break;
                case ActionMode.Edit:
                    if (obj is ExamSubject subject)
                    {
                        MainTeachingTip.Title = ResourcesHelper.GetString(ResourceKey.Edit) +
                                                ResourcesHelper.GetString(ResourceKey.ExamSubjects);
                        MainTeachingTip.IsOpen = true;
                        
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
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            
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
