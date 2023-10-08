using ExamBankSystem.Args;
using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using ExamBankSystem.Models;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static System.Net.Mime.MediaTypeNames;

namespace ExamBankSystem.Controls
{
    public sealed partial class ExamSubjectTip : UserControl
    {
        private string oldName;
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
            oldName = "";
            ExamSubject.Text = "";
            Mode = mode;
            switch (mode)
            {
                case ActionMode.Add:
                    MainTeachingTip.IsOpen = true;
                    MainTeachingTip.Title = ResourcesHelper.GetString(ResourceKey.Add);
                    break;
                case ActionMode.Edit:
                    if (obj is string name)
                    {
                        MainTeachingTip.Title = ResourcesHelper.GetString(ResourceKey.Edit);
                        MainTeachingTip.IsOpen = true;
                        oldName = name;
                        ExamSubject.Text = name;
                    }
                    break;
                case ActionMode.Delete:
                    if (obj is IList<object> items)
                    {
                        EventHelper.InvokeTopGridEvent(this,
                            new TopGridEventArg(
                                XamlHelper.CreateDeleteDialog(
                                    (sender, args) =>
                                    {
                                        foreach (var item in items.Cast<ExamSubject>())
                                        {
                                            DbHelper.DeleteExamSubject(item.Name);
                                        }

                                        EventHelper.InvokeTipPopup(this,
                                            ResourcesHelper.GetString(ResourceKey.DeleteSuccess),
                                            InfoBarSeverity.Success
                                        );
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
        /// 添加
        /// </summary>
        private void AddButton_Click(object sender, RoutedEventArgs e)
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

                    if (DbHelper.GetExamSubject(ExamSubject.Text) is ExamSubject subject)
                    {
                        EventHelper.InvokeTipPopup(this,
                            subject.Name + ResourcesHelper.GetString(ResourceKey.Exist),
                            InfoBarSeverity.Error
                        );
                        return;
                    }

                    DbHelper.InsertExamSubject(ExamSubject.Text);
                    EventHelper.InvokeTipPopup(this,
                        ResourcesHelper.GetString(ResourceKey.AddSuccess),
                        InfoBarSeverity.Success
                    );
                    break;
                case ActionMode.Edit:
                    DbHelper.UpdateExamSubjectName(ExamSubject.Text, oldName);

                    EventHelper.InvokeTipPopup(this,
                        ResourcesHelper.GetString(ResourceKey.EditSuccess),
                        InfoBarSeverity.Success
                    );
                    break;
            }

            Hide();
            RefreshEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}