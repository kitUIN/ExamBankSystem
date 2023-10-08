
using ExamBankSystem.Enums;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ExamBankSystem.Helpers
{
    public static class XamlHelper
    {
        
        /// <summary>
        /// 创建一个基础的ContentDialog
        /// </summary>
        public static ContentDialog CreateContentDialog()
        {
            var dialog = new ContentDialog();
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.DefaultButton = ContentDialogButton.Primary;
            return dialog;
        }

        /// <summary>
        /// 删除二次确认通知ContentDialog
        /// </summary>
        public static ContentDialog CreateDeleteDialog(
            Action<ContentDialog, ContentDialogButtonClickEventArgs> primaryAction = null,
            Action<ContentDialog, ContentDialogButtonClickEventArgs> closeAction = null)
        {
            return CreateThinkDialog(ResourcesHelper.GetString(ResourceKey.Delete),
                ResourcesHelper.GetString(ResourceKey.DeleteMessage),
                primaryAction,
                closeAction
            );
        }

        /// <summary>
        /// 删除二次确认通知ContentDialog
        /// </summary>
        public static ContentDialog CreateResetPasswordDialog(
            Action<ContentDialog, ContentDialogButtonClickEventArgs> primaryAction = null,
            Action<ContentDialog, ContentDialogButtonClickEventArgs> closeAction = null)
        {
            return CreateThinkDialog(ResourcesHelper.GetString(ResourceKey.ResetPassword),
                ResourcesHelper.GetString(ResourceKey.ResetPasswordMessage),
                primaryAction,
                closeAction
            );
        }
        /// <summary>
        /// 二次确认通知ContentDialog
        /// </summary>
        private static ContentDialog CreateThinkDialog(
            string title,
            string message,
            Action<ContentDialog, ContentDialogButtonClickEventArgs> primaryAction = null,
            Action<ContentDialog, ContentDialogButtonClickEventArgs> closeAction = null)
        {
            ContentDialog dialog = CreateContentDialog();
            dialog.Title = title;
            dialog.Content = message;
            dialog.IsPrimaryButtonEnabled = true;
            dialog.CloseButtonText = ResourcesHelper.GetString(ResourceKey.Cancel);
            dialog.PrimaryButtonText = ResourcesHelper.GetString(ResourceKey.Confirm);
            dialog.PrimaryButtonClick += (sender, args) =>
            {
                primaryAction?.Invoke(sender, args);
            };
            dialog.CloseButtonClick += (sender, args) =>
            {
                closeAction?.Invoke(sender, args);
            };
            return dialog;
        }
        
        
    }
}
