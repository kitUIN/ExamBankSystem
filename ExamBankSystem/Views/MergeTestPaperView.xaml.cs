using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using ExamBankSystem.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

namespace ExamBankSystem.Views
{
    public sealed partial class MergeTestPaperView : Page
    {
        private MergeTestPaperViewModel ViewModel { get; set; } = new MergeTestPaperViewModel();
        public MergeTestPaperView()
        {
            this.InitializeComponent();
        }

        private void SingleCButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TestPaperName.Text))
            {
                EventHelper.InvokeTipPopup(this,
                            ResourcesHelper.GetString(ResourceKey.TestPaperName) + 
                            ResourcesHelper.GetString(ResourceKey.NotNull),
                            InfoBarSeverity.Error
                        );
                return;
            }
            var ints = new List<int>
            {
                (int)SingleChoice.Value,
                (int)MultipleChoice.Value,
                (int)FillBlank.Value,
                (int)Judgment.Value,
                (int)ShortAnswer.Value,
                (int)Design.Value,
                (int)Program.Value
            };
            TextHelper.GeneratePaper(TestPaperName.Text, KnowledgePoints.Text,
                PaperRank.SelectedIndex + 1, ints);
            EventHelper.InvokeTipPopup(this,
                           ResourcesHelper.GetString(ResourceKey.GeneratePaperSuccess),
                           InfoBarSeverity.Success
                       );
        }

        private void ABButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {

        }
    }
}
