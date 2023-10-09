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
        /// <summary>
        /// 验证用户名是否合理
        /// </summary>
        private bool _userOk;

        /// <summary>
        /// 验证密码是否合理
        /// </summary>
        private bool _passwordOk = true;

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
            switch (mode)
            {
                case ActionMode.Add:
                    MainTeachingTip.IsOpen = true;
                    MainTeachingTip.Title = ResourcesHelper.GetString(ResourceKey.Add) + 
                                            ResourcesHelper.GetString(ResourceKey.User);
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
            if (!(Role.SelectedItem is FrameworkElement { Tag: string role })) return;
            DbHelper.InsertUser(User.Text, HashHelper.Hash_MD5_32(Password.Text), role);
            Hide();
            RefreshEvent?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 检测用户名是否合规
        /// </summary>
        private void User_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            // 不能为空
            if (string.IsNullOrEmpty(User.Text))
            {
                UserError.Text = ResourcesHelper.GetString(ResourceKey.UserNotNull);
                UserError.Visibility = Visibility.Visible;
                _userOk = false;
            }
            // 不能重复
            else if (DbHelper.GetUser(User.Text) != null)
            {
                UserError.Text = ResourcesHelper.GetString(ResourceKey.User) +
                                 ResourcesHelper.GetString(ResourceKey.Exist);
                UserError.Visibility = Visibility.Visible;
                _userOk = false;
            }
            // 不能有空格
            else if (User.Text.Contains(" "))
            {
                UserError.Text = ResourcesHelper.GetString(ResourceKey.UserNotSpace);
                UserError.Visibility = Visibility.Visible;
                _userOk = false;
            }
            else
            {
                _userOk = true;
                UserError.Visibility = Visibility.Collapsed;
            }
            AddButton.IsEnabled = _userOk && _passwordOk;
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
            if (string.IsNullOrEmpty(Password.Text))
            {
                PasswordError.Text = ResourcesHelper.GetString(ResourceKey.PasswordNull);
                PasswordError.Visibility = Visibility.Visible;
                _passwordOk = false;
            }
            // 不符合密码规则
            else if (Password.Text.IsNotPassword())
            {
                PasswordError.Text = ResourcesHelper.GetString(ResourceKey.PasswordNot);
                PasswordError.Visibility = Visibility.Visible;
                _passwordOk = false;
            }
            else
            {
                PasswordError.Visibility = Visibility.Collapsed;
                _passwordOk = true;
            }

            AddButton.IsEnabled = _userOk && _passwordOk;
        }
    }
}