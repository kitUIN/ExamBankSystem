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
using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using ExamBankSystem.Models;

namespace ExamBankSystem.Controls
{
    public sealed partial class TestPaperTip : UserControl
    {
        public TestPaperTip()
        {
            this.InitializeComponent();
        }

        public void Show(ActionMode mode, object obj = null)
        {
            switch (mode)
            {
                
                case ActionMode.Edit:
                    if (obj is KnowledgePoint point)
                    {
                        MainTeachingTip.Title = ResourcesHelper.GetString(ResourceKey.Edit) +
                                                ResourcesHelper.GetString(ResourceKey.KnowledgePoints);
                        MainTeachingTip.IsOpen = true;
                         
                    }

                    break;
                case ActionMode.Delete:

                    break;
            }
        }
    }
}