using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExamBankSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ExamBankSystem.Enums;
using ExamBankSystem.Extensions;
using ExamBankSystem.Models;
using ExamBankSystem.Utils;
using Microsoft.UI.Xaml.Controls;

namespace ExamBankSystem.ViewModels
{
    public partial class UserViewModel:ObservableObject
    {
        /// <summary>
        /// 旧密码错误提示
        /// </summary>
        [ObservableProperty]
        private bool oldPasswordErrorVisible;
        /// <summary>
        /// 新密码错误提示
        /// </summary>
        [ObservableProperty]
        private bool newPasswordErrorVisible;
        /// <summary>
        /// 旧密码错误提示内容
        /// </summary>
        [ObservableProperty]
        private string oldPasswordError;
        /// <summary>
        /// 新密码错误提示内容
        /// </summary>
        [ObservableProperty]
        private string newPasswordError;
        /// <summary>
        /// 旧密码
        /// </summary>
        [ObservableProperty]
        private string oldPassword;
        /// <summary>
        /// 新密码
        /// </summary>
        [ObservableProperty]
        private string newPassword;
        /// <summary>
        /// 登出
        /// </summary>
        [RelayCommand]
        private void Logout()
        {
            EventHelper.InvokeLogoutEvent(this);
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        [RelayCommand]
        private void ChangePassword()
        {
            if (NewPasswordErrorVisible || OldPasswordErrorVisible) return;
            if (DbHelper.GetUser(CurrentData.CurrentUser.Id) is { } user)
            {
                if (user.Password == HashHelper.Hash_MD5_32(OldPassword))
                {
                    DbHelper.UpdateUserPassword(user.Id,HashHelper.Hash_MD5_32(NewPassword));
                    EventHelper.InvokeTipPopup(this,
                        ResourcesHelper.GetString(ResourceKey.ChangePasswordSuccess),
                        InfoBarSeverity.Success
                    );
                    EventHelper.InvokeLogoutEvent(this);
                }
                else
                {
                    EventHelper.InvokeTipPopup(this,
                        ResourcesHelper.GetString(ResourceKey.OldPasswordError),
                        InfoBarSeverity.Error
                    );
                }
            }
        }
        /// <summary>
        /// 检测新密码
        /// </summary>
        public void NewPassword_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(sender is TextBox box)) return;
            // 空
            if (box.Text == "")
            {
                NewPasswordError= ResourcesHelper.GetString(ResourceKey.PasswordNull);
                NewPasswordErrorVisible = true;
            }
            // 不符合密码规则
            else if (box.Text.IsNotPassword())
            {
                NewPasswordError = ResourcesHelper.GetString(ResourceKey.PasswordNot);
                NewPasswordErrorVisible = true;
            }
            else
            {
                NewPasswordErrorVisible = false;
            }
        }
        /// <summary>
        /// 检测旧密码
        /// </summary>
        public void OldPassword_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(sender is TextBox box)) return;
            // 空
            if (box.Text == "")
            {
                OldPasswordError= ResourcesHelper.GetString(ResourceKey.PasswordNull);
                OldPasswordErrorVisible = true;
            }
            // 不符合密码规则
            else if (box.Text.IsNotPassword())
            {
                OldPasswordError = ResourcesHelper.GetString(ResourceKey.PasswordNot);
                OldPasswordErrorVisible = true;
            }
            else
            {
                OldPasswordErrorVisible = false;
            }
        }
    }
}
