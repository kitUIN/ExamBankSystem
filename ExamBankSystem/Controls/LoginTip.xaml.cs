using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using ExamBankSystem.Utils;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
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
        public void Open()
        {
            LoginTeachingTip.IsOpen = true;
            var name = ConfigHelper.GetString(ConfigKey.LastUserName);
            var pwd = ConfigHelper.GetString(ConfigKey.LastUserPassword);
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(pwd))
            {
                
                Remember.IsChecked = true;
                User.Text = name;
                Password.Password = pwd;
                Button_Click(this,null);
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
            LoginTeachingTip.IsOpen = false;
        }
        /// <summary>
        /// 点击登录
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
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
            var user = DbHelper.GetUser(User.Text);
            if (user == null)
            {
                EventHelper.InvokeTipPopup(this,
                    ResourcesHelper.GetString(ResourceKey.UserNull),
                    InfoBarSeverity.Error
                    );
                return;
            }
            if (user.Password != Password.Password)
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
                DbHelper.UpdateUserLoginTime(user.Name);
                CurrentData.CurrentUser = DbHelper.GetUser(User.Text);
                if (Remember.IsChecked.Value)
                {
                    ConfigHelper.Set(ConfigKey.LastUserName, User.Text);
                    ConfigHelper.Set(ConfigKey.LastUserPassword, Password.Password);
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
            if(e.Key == Windows.System.VirtualKey.Enter)
            {
                Button_Click(sender, e);
            }
        }
    }
}
