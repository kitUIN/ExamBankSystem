﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExamBankSystem.Helpers;
using System.Threading.Tasks;
using ExamBankSystem.Enums;
using ExamBankSystem.Extensions;
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
        [NotifyPropertyChangedFor(nameof(ChangePasswordEnable))]
        private bool oldPasswordErrorVisible = true;
        /// <summary>
        /// 新密码错误提示
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ChangePasswordEnable))]
        private bool newPasswordErrorVisible = true;
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
        /// 修改密码按钮的启用
        /// </summary>
        public bool ChangePasswordEnable => !NewPasswordErrorVisible && !OldPasswordErrorVisible;
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
        private async Task ChangePasswordAsync()
        {
            var user = await DbHelper.GetUserAsync(CurrentData.CurrentUser.Id);
            if (user == null) return;
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
            else if(box.Text == OldPassword)
            {
                NewPasswordError = ResourcesHelper.GetString(ResourceKey.PasswordDifferent);
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
            if (string.IsNullOrEmpty(box.Text))
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
