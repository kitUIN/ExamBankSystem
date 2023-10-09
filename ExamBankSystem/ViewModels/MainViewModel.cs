using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using ExamBankSystem.Models;
using ExamBankSystem.Utils;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

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
            Logout();
        }
        /// <summary>
        /// 登出
        /// </summary>
        public void Logout()
        {
            FootItems.Clear();
            MenuItems.Clear();
            FootItems.Add(new Category()
            {
                Name = ResourcesHelper.GetString(ResourceKey.Login),
                Tag = CategoryTag.Login,
                Icon = new FontIcon() { Glyph = "\uE77B" }
            });
        }
        /// <summary>
        /// 登陆后初始化
        /// </summary>
        public void Init()
        {
            FootItems.Clear();
            MenuItems.Clear();
            FootItems.Add(new Category()
            {
                Name = CurrentData.CurrentUser.Name,
                Tag = CategoryTag.User,
                Icon = new FontIcon() { Glyph = "\uE77B" }
            });
            if (CurrentData.CurrentUser.Role == UserRole.Admin)
            {
                MenuItems.Add(new Category()
                {
                    Name = ResourcesHelper.GetString(ResourceKey.ExamSubjects),
                    Tag = CategoryTag.ExamSubject,
                    Icon = new FontIcon() { Glyph = "\uEA41" }
                });
                MenuItems.Add(new Category()
                {
                    Name = ResourcesHelper.GetString(ResourceKey.KnowledgePoints),
                    Tag = CategoryTag.KnowledgePoint,
                    Icon = new FontIcon() { Glyph = "\uE70B" }
                });
                MenuItems.Add(new Category()
                {
                    Name = ResourcesHelper.GetString(ResourceKey.Questions),
                    Tag = CategoryTag.Question,
                    Icon = new FontIcon() { Glyph = "\uEADF" }
                });
                MenuItems.Add(new Category()
                {
                    Name = ResourcesHelper.GetString(ResourceKey.UserManager),
                    Tag = CategoryTag.UserManager,
                    Icon = new FontIcon() { Glyph = "\uE779" }
                });
            }
            if (CurrentData.CurrentUser.Role != UserRole.Student)
            {
                MenuItems.Add(new Category()
                {
                    Name = ResourcesHelper.GetString(ResourceKey.TestPapers),
                    Icon = new FontIcon() { Glyph = "\uE82D" },
                    Tag = CategoryTag.TestPaper,
                    Children = new ObservableCollection<Category>()
                {
                    new Category()
                    {
                        Name = ResourcesHelper.GetString(ResourceKey.Find),
                        Tag = CategoryTag.FindTestPaper,
                        Icon = new FontIcon() { Glyph = "\uE721" },
                    },
                    new Category()
                    {
                        Name = ResourcesHelper.GetString(ResourceKey.Merge),
                        Tag = CategoryTag.MergeTestPaper,
                        Icon = new FontIcon() { Glyph = "\uEA61" },
                    },
                    new Category()
                    {
                        Name = ResourcesHelper.GetString(ResourceKey.Manage),
                        Tag = CategoryTag.ManageTestPaper,
                        Icon = new FontIcon() { Glyph = "\uE9F5" },
                    },
                }
                });
            }
                
        }

    }
}
