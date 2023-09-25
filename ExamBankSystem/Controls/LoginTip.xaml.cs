using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using ExamBankSystem.Utils;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        }
        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            LoginTeachingTip.IsOpen = false;
        }

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
            var user = DbHelper.GetUser(User.Text);
            if (string.IsNullOrEmpty(Password.Password))
            {
                EventHelper.InvokeTipPopup(this,
                    ResourcesHelper.GetString(ResourceKey.PasswordNull),
                    InfoBarSeverity.Error
                    );
                return;
            }
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
                CurrentData.CurrentUser = user;
                Hide();
                EventHelper.InvokeLoginEvent(this);
            }
        }
    }
}
