using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using ApiFormat.Group;
using ClientLibAPI;
using Prism.Commands;
using SplitList.Models;
using SplitList.Views;
using Xamarin.Forms;

namespace SplitList.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        public UserViewModel(INavigation nav, Page page, int groupId, string userId) : base(nav, page, groupId, userId)
        {
        }

        private ObservableCollection<Group> _groups;

        public ObservableCollection<Group> Groups
        {
            get => _groups;
            set => SetProperty(ref _groups, value);

        }
        private ICommand _editGroupCommand;

        public ICommand EditGroupCommand
        {
            get => _editGroupCommand ?? (_editGroupCommand = new DelegateCommand<object>(EditGroupCommandExecute));
        }

        public async void EditGroupCommandExecute(object sender)
        {
            if(sender is Group group)
                await Navigation.PushAsync(new GroupEditView(GroupId, UserId));
        }

        public override async void OnAppearingExecute()
        {
            var user = mapper.Map<User>(await SerializerUser.GetUserById(UserId));
            Groups = user.Groups;
        }
    }
}
