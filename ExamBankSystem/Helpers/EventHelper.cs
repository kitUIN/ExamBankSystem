using ExamBankSystem.Args;
using ExamBankSystem.Controls;
using ExamBankSystem.Enums;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamBankSystem.Helpers
{
    /// <summary>
    /// 事件通知帮助类
    /// </summary>
    public static class EventHelper
    {
        /// <summary>
        /// 登录事件
        /// </summary>
        public static event EventHandler LoginEvent;
        /// <summary>
        /// 顶部窗体通知事件
        /// </summary>
        public static event EventHandler<TopGridEventArg> TopGridEvent;
        /// <summary>
        /// 触发登录事件
        /// </summary>
        /// <param name="sender"></param>
        public static void InvokeLoginEvent(object sender)
        {
            LoginEvent?.Invoke(sender, EventArgs.Empty);
        }
        /// <summary>
        /// 触发顶部窗体通知事件
        /// </summary>
        /// <param name="sender"></param>
        public static void InvokeTopGridEvent(object sender, TopGridEventArg arg)
        {
            TopGridEvent?.Invoke(sender, arg);
        }
        /// <summary>
        /// 触发顶部窗体通知事件
        /// </summary>
        public static void InvokeTipPopup(object sender,string text="", InfoBarSeverity type = InfoBarSeverity.Informational, double displaySeconds = 2)
        {
            TopGridEvent?.Invoke(sender, 
                new TopGridEventArg(
                        new TipPopup(text, type,displaySeconds),
                        TopGridMode.Tip));
        }
    }
}
