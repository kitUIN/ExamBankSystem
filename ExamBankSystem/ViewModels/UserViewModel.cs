using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExamBankSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamBankSystem.ViewModels
{
    public partial class UserViewModel:ObservableObject
    {
        [RelayCommand]
        private void Logout()
        {
            EventHelper.InvokeLogoutEvent(this);
        }
        [RelayCommand]
        private void ChangePassword()
        {

        }
    }
}
