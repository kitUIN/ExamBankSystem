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

        private void StackPanel_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            SingleChoice.Maximum = DbHelper.GetQuestionCountAsync("type", 0) ;
            SingleChoice.IsEnabled = SingleChoice.Maximum > 0;
            MultipleChoice.Maximum = DbHelper.GetQuestionCountAsync("type", 1) ;
            MultipleChoice.IsEnabled = MultipleChoice.Maximum > 0;
            FillBlank.Maximum = DbHelper.GetQuestionCountAsync("type", 2) ;
            FillBlank.IsEnabled = FillBlank.Maximum > 0;
            Judgment.Maximum = DbHelper.GetQuestionCountAsync("type", 3) ;
            Judgment.IsEnabled = Judgment.Maximum > 0;
            ShortAnswer.Maximum = DbHelper.GetQuestionCountAsync("type", 4) ;
            ShortAnswer.IsEnabled = ShortAnswer.Maximum > 0;
            Design.Maximum = DbHelper.GetQuestionCountAsync("type", 5);
            Design.IsEnabled = Design.Maximum > 0;
            Program.Maximum = DbHelper.GetQuestionCountAsync("type", 6) ;
            Program.IsEnabled = Program.Maximum > 0;
        }
    }
}
