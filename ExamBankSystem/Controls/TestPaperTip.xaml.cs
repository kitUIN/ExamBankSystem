using System;
using System.Collections.ObjectModel;
using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using ExamBankSystem.Models;
using ExamBankSystem.Args;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;

namespace ExamBankSystem.Controls
{
    public sealed partial class TestPaperTip : UserControl
    {
        public ObservableCollection<QuestionPaper> RightItems { get; } = new ObservableCollection<QuestionPaper>();
        /// <summary>
        /// 请求刷新事件
        /// </summary>
        public event EventHandler RefreshEvent;
        public TestPaperTip()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            MainTeachingTip.IsOpen = false;
        }
        public void Show(ActionMode mode, object obj = null)
        {
            switch (mode)
            {
                case ActionMode.Edit:
                    if (obj is TestPaper testPaper)
                    {
                        MainTeachingTip.Title = ResourcesHelper.GetString(ResourceKey.Edit) +
                                                ResourcesHelper.GetString(ResourceKey.KnowledgePoints);
                        MainTeachingTip.IsOpen = true;
                        CurrentQuestionType.Text = "";
                        RightItems.Clear();
                        foreach (var item in DbHelper.GetQuestionsPapersByTestPaper(testPaper.Id))
                        {
                            RightItems.Add(item);
                        }
                    }
                    break;
                case ActionMode.Delete:
                    if (obj is TestPaper paper)
                    {
                        EventHelper.InvokeTopGridEvent(this,
                            new TopGridEventArg(
                                XamlHelper.CreateDeleteDialog((sender, args) =>
                                    {
                                        DbHelper.DeleteById<TestPaper>(paper.Id);

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
        /// 替换
        /// </summary>
        private void ReplaceButton_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
        private int CurrentQuestionTypeId { get; set; }
        private void RightList_OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (RightList.SelectedItem is QuestionPaper paper)
            {
                CurrentQuestionType.Text = paper.Question.TypeToString(paper.Question.Type);
                CurrentQuestionTypeId = (int)paper.Question.Type;
                SearchBox.Text = "";
            }
        }

        private async void SearchBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput&& !string.IsNullOrEmpty(CurrentQuestionType.Text) )
            {
                sender.ItemsSource = await DbHelper.SearchQuestionWithType(CurrentQuestionTypeId, sender.Text);
            }
        }
    }
}