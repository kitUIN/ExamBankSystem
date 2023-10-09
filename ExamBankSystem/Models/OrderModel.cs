using System.Data;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ExamBankSystem.Models
{
    public partial class OrderModel: ObservableObject
    {
        /// <summary>
        /// 序号
        /// </summary>
        [ObservableProperty]
        private long order;
        public OrderModel(IDataRecord query){ }
        public OrderModel() { }
    }
}