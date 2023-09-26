using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExamBankSystem.Args;
using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;
using ExamBankSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamBankSystem.ViewModels
{
    public partial class TestPaperViewModel: ObservableObject
    {
        /// <summary>
        /// 操作事件
        /// </summary>
        public event EventHandler<ActionEventArg> ActionEvent;
        public ObservableCollection<TestPaper> TestPapers { get; set; } = new ObservableCollection<TestPaper>();
        public TestPaperViewModel()
        {
            Refresh();
        }
        /// <summary>
        /// 刷新列表
        /// </summary>
        public void Refresh()
        {
            TestPapers.Clear();
            foreach (var item in DbHelper.GetTestPapers())
            {
                TestPapers.Add(item);
            }
        }
        
        [RelayCommand]
        private void Edit(object obj)
        {
            if (obj is ExamSubject subject)
            {
                ActionEvent?.Invoke(this, new ActionEventArg()
                {
                    TipMode = TipMode.Show,
                    ActionMode = ActionMode.Edit,
                    Source = subject.Name
                });
            }
        }
        [RelayCommand]
        private void Add()
        {
            ActionEvent?.Invoke(this, new ActionEventArg()
            {
                TipMode = TipMode.Show,
                ActionMode = ActionMode.Add,
            });
        }
        [RelayCommand]
        private void Delete(object obj)
        {
            ActionEvent?.Invoke(this, new ActionEventArg()
            {
                TipMode = TipMode.Show,
                ActionMode = ActionMode.Delete,
                Source = obj
            });
        }
    }
}
