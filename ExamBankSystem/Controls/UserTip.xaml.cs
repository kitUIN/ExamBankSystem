using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ExamBankSystem.Args;
using ExamBankSystem.Enums;
using ExamBankSystem.Extensions;
using ExamBankSystem.Helpers;
using ExamBankSystem.Models;
using Microsoft.UI.Xaml.Controls;

namespace ExamBankSystem.Controls
{
    public sealed partial class UserTip : UserControl
    {
        private ActionMode Mode { get; set; }
        /// <summary>
        /// 验证用户名是否合理
        /// </summary>
        private bool userOk = false;
        /// <summary>
        /// 验证密码是否合理
        /// </summary>
        private bool passwordOk = true;

        /// <summary>
        /// 请求刷新事件
        /// </summary>
        public event EventHandler RefreshEvent;

        public UserTip()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void Show(ActionMode mode, object obj = null)
        {
            Mode = mode;
            switch (mode)
            {
                case ActionMode.Add:
                    MainTeachingTip.IsOpen = true;
                    MainTeachingTip.Title = ResourcesHelper.GetString(ResourceKey.Add);
                    User.Text = "";
                    Password.Text = "qust123";
                    break;
                case ActionMode.Reset:
                    if (obj is IList<object> resets)
                    {
                        EventHelper.InvokeTopGridEvent(this,
                            new TopGridEventArg(
                                XamlHelper.CreateResetPasswordDialog(
                                    (sender, args) =>
                                    {
                                        foreach (var item in resets.Cast<User>())
                                        {
                                            DbHelper.ResetUserPassword(item.Id);
                                        }

                                        EventHelper.InvokeTipPopup(this,
                                            ResourcesHelper.GetString(ResourceKey.User) +
                                            ResourcesHelper.GetString(ResourceKey.Password) +
                                            ResourcesHelper.GetString(ResourceKey.ResetSuccess),
                                            InfoBarSeverity.Success
                                        );
                                        RefreshEvent?.Invoke(this, EventArgs.Empty);
                                    }
                                ),
                                TopGridMode.ContentDialog));
                    }

                    break;
                case ActionMode.Delete:
                    if (obj is IList<object> items)
                    {
                        EventHelper.InvokeTopGridEvent(this,
                            new TopGridEventArg(
                                XamlHelper.CreateDeleteDialog(
                                    (sender, args) =>
                                    {
                                        foreach (var item in items.Cast<User>())
                                        {
                                            DbHelper.DeleteUser(item.Id);
                                        }

                                        EventHelper.InvokeTipPopup(this,
                                            ResourcesHelper.GetString(ResourceKey.User) +
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
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            MainTeachingTip.IsOpen = false;
        }

        /// <summary>
        /// 添加
        /// </summary>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // 用户名是否为空
            if (string.IsNullOrEmpty(User.Text))
            {
                EventHelper.InvokeTipPopup(this,
                    ResourcesHelper.GetString(ResourceKey.UserNull),
                    InfoBarSeverity.Error
                );
                return;
            }

            // 密码是否为空
            if (string.IsNullOrEmpty(Password.Text))
            {
                EventHelper.InvokeTipPopup(this,
                    ResourcesHelper.GetString(ResourceKey.PasswordNull),
                    InfoBarSeverity.Error
                );
                return;
            }

            if (!(Role.SelectedItem is FrameworkElement { Tag: string role })) return;
            DbHelper.InsertUser(User.Text, HashHelper.Hash_MD5_32(Password.Text), role);
            Hide();
            RefreshEvent?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 检测用户名是否存在
        /// </summary>
        private void User_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            userOk = DbHelper.GetUser(User.Text) is null;
            AddButton.IsEnabled = userOk && passwordOk;
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        /// <summary>
        /// 检测密码是否合理
        /// </summary>
        private void Password_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            // 不能为空
            if (Password.Text == "")
            {
                PasswordError.Text = ResourcesHelper.GetString(ResourceKey.PasswordNull);
                PasswordError.Visibility = Visibility.Visible;
                passwordOk = false;
            }
            // 不符合密码规则
            else if (Password.Text.IsNotPassword())
            {
                PasswordError.Text = ResourcesHelper.GetString(ResourceKey.PasswordNot);
                PasswordError.Visibility = Visibility.Visible;
                passwordOk = false;
            }
            else
            {
                PasswordError.Visibility = Visibility.Collapsed;
                passwordOk = true;
            }

            AddButton.IsEnabled = userOk && passwordOk;
        }
    }
}