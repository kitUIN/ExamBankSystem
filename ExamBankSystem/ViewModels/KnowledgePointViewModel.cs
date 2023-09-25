using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExamBankSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamBankSystem.ViewModels
{
    public partial class KnowledgePointViewModel: ObservableObject
    {
        [ObservableProperty]
        private bool canEdit = false;
        [ObservableProperty]
        private bool canDelete = false;
        /// <summary>
        /// 知识点列表
        /// </summary>
        public ObservableCollection<KnowledgePoint> KnowledgePoints { get; set; } = new ObservableCollection<KnowledgePoint>();
        /// <summary>
        /// 列表选择变化响应
        /// </summary>
        public void SelectionChanged(int count)
        {
            CanEdit = count == 1;
            CanDelete = count > 0;
        }
        [RelayCommand]
        private void Edit()
        {

        }
        [RelayCommand]
        private void Add()
        {

        }
        [RelayCommand]
        private void Delete()
        {

        }
    }
}
