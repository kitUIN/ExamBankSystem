using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using ExamBankSystem.Utils;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ExamBankSystem.Controls
{
    public sealed partial class LoginTip : UserControl
    {
        /// <summary>
        /// 登录界面
        /// </summary>
        public LoginTip()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void Show()
        {
            MainTeachingTip.IsOpen = true;
            var name = ConfigHelper.GetString(ConfigKey.LastUserName);
            var pwd = ConfigHelper.GetString(ConfigKey.LastUserPassword);
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(pwd))
            {
                
                Remember.IsChecked = true;
                User.Text = name;
                Password.Password = pwd;
                LoginButton_Click(this,null);
            }
            else
            {
                Remember.IsChecked = false;
                User.Text = "";
                Password.Password = "";
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
        /// 点击登录
        /// </summary>
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(User.Text))
            {
                EventHelper.InvokeTipPopup(this,
                    ResourcesHelper.GetString(ResourceKey.UseNameNull),
                    InfoBarSeverity.Error
                    );
                return;
            }
            
            if (string.IsNullOrEmpty(Password.Password))
            {
                EventHelper.InvokeTipPopup(this,
                    ResourcesHelper.GetString(ResourceKey.PasswordNull),
                    InfoBarSeverity.Error
                    );
                return;
            }
            var user = await DbHelper.GetUserAsync(User.Text);
            if (user == null)
            {
                EventHelper.InvokeTipPopup(this,
                    ResourcesHelper.GetString(ResourceKey.UserNull),
                    InfoBarSeverity.Error
                    );
                return;
            }
            var password = Password.Password.Length == 32 ? Password.Password:HashHelper.Hash_MD5_32(Password.Password);
            Debug.WriteLine(password);
            if (password != user.Password)
            {
                EventHelper.InvokeTipPopup(this,
                    ResourcesHelper.GetString(ResourceKey.PasswordError),
                    InfoBarSeverity.Error
                    );
            }
            else
            {
                EventHelper.InvokeTipPopup(this,
                    ResourcesHelper.GetString(ResourceKey.LoginSuccess),
                    InfoBarSeverity.Success
                    );
                DbHelper.UpdateUserLoginTime(user.Id);
                CurrentData.CurrentUser = await DbHelper.GetUserAsync(User.Text);
                if (Remember.IsChecked.Value)
                {
                    ConfigHelper.Set(ConfigKey.LastUserName, User.Text);
                    ConfigHelper.Set(ConfigKey.LastUserPassword, password);
                }
                else
                {
                    ConfigHelper.Set(ConfigKey.LastUserName, "");
                    ConfigHelper.Set(ConfigKey.LastUserPassword, "");
                }
                Hide();
                EventHelper.InvokeLoginEvent(this);
            }
        }
         
        /// <summary>
        /// 回车登录
        /// </summary>
        private void Password_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if( e.Key == Windows.System.VirtualKey.Enter)
            {
                LoginButton_Click(sender, e);
            }
        }
    }
}
