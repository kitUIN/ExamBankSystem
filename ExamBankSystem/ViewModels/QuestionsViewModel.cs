using CommunityToolkit.Mvvm.ComponentModel;
using ExamBankSystem.Models;
using Windows.UI.Xaml.Controls;

namespace ExamBankSystem.ViewModels
{
    public partial class QuestionsViewModel : DataTableBase<Question>
    {
        public QuestionsViewModel(string searchCol= "subjectId") : base(searchCol)
        {
        }
        [ObservableProperty]
        private bool search1 = true;
        [ObservableProperty]
        private bool search2;
        [ObservableProperty]
        private bool search3;
        /// <summary>
        /// 
        /// </summary>
        public void SelectionChanged(int count)
        {
            CanDelete = CanEdit = count == 1;
        }

        public void SearchType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(sender is ComboBox searcher)
            {
                if (searcher.SelectedIndex == 0)
                {
                    SearchCol = "subjectId";
                    Search1 = true;
                    Search2 = Search3 = false;
                }
                else if (searcher.SelectedIndex == 1)
                {
                    SearchCol = "rank";
                    SearchText = "0";
                    Search2 = true;
                    Search1 = Search3 = false;
                }
                else if (searcher.SelectedIndex == 2)
                {
                    SearchCol = "type";
                    SearchText = "0";
                    Search3 = true;
                    Search1 = Search2 = false;
                }
                else if (searcher.SelectedIndex == 3)
                {
                    SearchCol = "knowledgeId";
                    Search1 = true;
                    Search2 = Search3 = false;
                }
            }
        }
    }
}
