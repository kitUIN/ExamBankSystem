using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using ExamBankSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ExamBankSystem.ViewModels
{
    /// <summary>
    /// 主页面视图模型
    /// </summary>
    public class MainViewModel
    {
        /// <summary>
        /// 是否登录
        /// </summary>
        public static bool IsLogin { get; set; } = false;
        /// <summary>
        /// 导航栏分类
        /// </summary>
        public ObservableCollection<Category> MenuItems { get; set; } = new ObservableCollection<Category>();
        /// <summary>
        /// 导航栏底部分类
        /// </summary>
        public ObservableCollection<Category> FootItems { get; set; } = new ObservableCollection<Category>();

        public MainViewModel()
        {
            FootItems.Add(new Category()
            {
                Name = ResourcesHelper.GetString(ResourceKey.Login),
                Tag = "Login",
                Icon =  new FontIcon() { Glyph = "\uE77B" }
            });
        }
        /// <summary>
        /// 登陆后初始化
        /// </summary>
        public void Init()
        {
            FootItems.Clear();
            MenuItems.Clear();
            MenuItems.Add(new Category()
            {
                Name = ResourcesHelper.GetString(ResourceKey.ExamSubjects),
                Tag = "ExamSubjects",
                Icon = new FontIcon() { Glyph = "\uEA41" }
            });
            MenuItems.Add(new Category()
            {
                Name = ResourcesHelper.GetString(ResourceKey.KnowledgePoints),
                Tag = "KnowledgePoints",
                Icon = new FontIcon() { Glyph = "\uE70B" }
            });
            MenuItems.Add(new Category()
            {
                Name = ResourcesHelper.GetString(ResourceKey.Questions),
                Tag = "Questions",
                Icon = new FontIcon() { Glyph = "\uE721" }
            });
            MenuItems.Add(new Category()
            {
                Name = ResourcesHelper.GetString(ResourceKey.TestPapers),
                Tag = "TestPapers",
                Icon = new FontIcon() { Glyph = "\uE82D" }
            });
        }

    }
}
