using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Prism.Commands;
using SplitList.Models;
using Xamarin.Forms;

namespace SplitList.ViewModels
{
    public class UserViewModel : BindableBase
    {
        public UserViewModel(User user)
        {
            User = user;
        }

        private User _user;

        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private ICommand _removeGroupCommand;

        public ICommand RemoveGroupCommand
        {
            get => _removeGroupCommand ?? (_removeGroupCommand = new DelegateCommand<object>(RemoveGroupCommandExecute));
        }

        public async void RemoveGroupCommandExecute(object sender)
        {
            var group = sender as Group;
            var result = await Application.Current.MainPage.DisplayAlert("Warning", "Are you sure you want to leave this group?", "Yes", "Cancel");
            if (result)
                User.Groups.Remove(group);
        }
    }
}
