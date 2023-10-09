using ExamBankSystem.Enums;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel; 

namespace ExamBankSystem.Models
{
    /// <summary>
    /// 导航栏分类
    /// </summary>
    public class Category
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public CategoryTag Tag { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public IconElement Icon { get; set; }
        /// <summary>
        /// 成员
        /// </summary>
        public ObservableCollection<Category> Children { get; set; }
    }
}
