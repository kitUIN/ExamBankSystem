using ExamBankSystem.Enums;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace ExamBankSystem.Models
{
    /// <summary>
    /// 导航栏分类
    /// </summary>
    public class Category
    {
        public string Name { get; set; }
        public CategoryTag Tag { get; set; }
        public IconElement Icon { get; set; }
        public ObservableCollection<Category> Children { get; set; }
    }
}
