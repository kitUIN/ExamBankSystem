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
        private List<string> files = new List<string>();
        private void Gen_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
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
                PaperRank.SelectedIndex + 1, ints, files, GenType.SelectedIndex);
            EventHelper.InvokeTipPopup(this,
                           ResourcesHelper.GetString(ResourceKey.GeneratePaperSuccess),
                           InfoBarSeverity.Success
                       );
        }

        private void GenType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var t = GenType.SelectedIndex + 1;
            SingleChoice.Maximum = DbHelper.GetQuestionCountAsync("type", 0) / t;
            SingleChoice.IsEnabled = SingleChoice.Maximum > 0;
            MultipleChoice.Maximum = DbHelper.GetQuestionCountAsync("type", 1) / t;
            MultipleChoice.IsEnabled = MultipleChoice.Maximum > 0;
            FillBlank.Maximum = DbHelper.GetQuestionCountAsync("type", 2) / t;
            FillBlank.IsEnabled = FillBlank.Maximum > 0;
            Judgment.Maximum = DbHelper.GetQuestionCountAsync("type", 3) / t;
            Judgment.IsEnabled = Judgment.Maximum > 0;
            ShortAnswer.Maximum = DbHelper.GetQuestionCountAsync("type", 4) / t;
            ShortAnswer.IsEnabled = ShortAnswer.Maximum > 0;
            Design.Maximum = DbHelper.GetQuestionCountAsync("type", 5) / t;
            Design.IsEnabled = Design.Maximum > 0;
            Program.Maximum = DbHelper.GetQuestionCountAsync("type", 6) / t;
            Program.IsEnabled = Program.Maximum > 0;
        }

        private async void SelectWords_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            files = await FileHelper.GetMultipleDocxAsync();
            SelectWordsText.Text = string.Join(",", files);
        }
    }
}
