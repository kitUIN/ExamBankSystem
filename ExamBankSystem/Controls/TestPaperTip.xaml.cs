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

        private void Refresh(int id)
        {
            RightItems.Clear();
            foreach (var item in DbHelper.GetQuestionsPapersByTestPaper(id))
            {
                RightItems.Add(item);
            }
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
                        Refresh(testPaper.Id);
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
                                        DbHelper.DeleteQuestionPaperByTestPaperId(paper.Id);
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
            if (CurrentPaper == null)
            {
                EventHelper.InvokeTipPopup(this,"未选中" ,
                    InfoBarSeverity.Error
                );
            }
            if (string.IsNullOrEmpty(SearchBox.Text))
            {
                EventHelper.InvokeTipPopup(this,
                                            ResourcesHelper.GetString(ResourceKey.Id)+
                                            ResourcesHelper.GetString(ResourceKey.NotNull   ) ,
                                            InfoBarSeverity.Error
                                        ); 
            }
            else
            {
                try
                {
                    var index = Convert.ToInt32(SearchBox.Text);
                    if (DbHelper.GetQuestionsPapers(CurrentPaper.TestPaperId,index) == null)
                    {
                        DbHelper.UpdateQuestionPaper(CurrentPaper.TestPaperId, CurrentPaper.QuestionIndex,index);
                    }
                    else
                    {
                        EventHelper.InvokeTipPopup(this,
                            ResourcesHelper.GetString(ResourceKey.Id)+
                            ResourcesHelper.GetString(ResourceKey.Exist) ,
                            InfoBarSeverity.Error
                        );
                    }
                }
                catch
                {
                    EventHelper.InvokeTipPopup(this,
                        ResourcesHelper.GetString(ResourceKey.Id)+
                         "不合理" ,
                        InfoBarSeverity.Error
                    );
                }
            }
            Hide();
            if (CurrentPaper != null)
            {
                
                Refresh(CurrentPaper.TestPaperId);
            }
        }
        private int CurrentQuestionTypeId { get; set; }
        private QuestionPaper CurrentPaper { get; set; } 
        private void RightList_OnItemClick(object sender, ItemClickEventArgs e)
        {
        } 

        private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }

        private void RightList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (RightList.SelectedItem is QuestionPaper paper)
            {
                CurrentQuestionType.Text = paper.Question.TypeToString(paper.Question.Type);
                CurrentQuestionTypeId = (int)paper.Question.Type;
                CurrentPaper =  paper;
                SearchBox.Text = "";
            }
        }
    }
}