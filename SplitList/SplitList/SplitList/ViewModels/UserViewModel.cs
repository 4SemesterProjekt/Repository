using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Prism.Commands;
using SplitList.Models;
using SplitList.Views;
using Xamarin.Forms;

namespace SplitList.ViewModels
{
    public class UserViewModel : BindableBase
    {
        public UserViewModel(User user, INavigation navigation)
        {
            User = user;
            Navigation = navigation;
        }

        public INavigation Navigation { get; set; }
        private User _user;

        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private ICommand _editGroupCommand;

        public ICommand EditGroupCommand
        {
            get => _editGroupCommand ?? (_editGroupCommand = new DelegateCommand<object>(EditGroupCommandExecute));
        }

        public async void EditGroupCommandExecute(object sender)
        {
            if(sender is Group group)
                await Navigation.PushAsync(new GroupEditView(group.GroupId));
        }
    }
}
