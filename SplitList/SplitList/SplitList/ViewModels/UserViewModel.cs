using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using ApiFormat.Group;
using ApiFormat.User;
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
            User = new User();
        }

        private User _user;

        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private ICommand _addNewGroupCommand;

        public ICommand AddNewGroupCommand => _addNewGroupCommand ?? (_addNewGroupCommand = new DelegateCommand(AddNewGroupExecute));

        public async void AddNewGroupExecute()
        {
            var groupName =
                await Page.DisplayPromptAsync("New group", "Input the name for the new group", "Create", "Cancel");
            if (groupName != null)
            {
                Group newGroup = new Group() { Name = groupName };
                newGroup.Users.Add(User);
                var groupFromDb = await SerializerGroup.CreateGroup(mapper.Map<GroupDTO>(newGroup));
                User.Groups.Add(mapper.Map<Group>(groupFromDb));
            }

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
            User = mapper.Map<User>(await SerializerUser.GetUserById(UserId));
            foreach (var userGroup in User.Groups)
            {
                userGroup.LeaveGroupEvent += HandleLeaveGroupEvent;
            }
        }
        public async void HandleLeaveGroupEvent(object source, EventArgs e)
        {
            var result = await Page.DisplayAlert("Alert", "Are you sure you want to leave this group?", "Yes",
                "Cancel");
            if (!result) return;
            if (source is User userIn)
            {
                for (int i = 0; i < userIn.Groups.Count; i++)
                {
                    if (userIn.Groups[i].Name.ToLower() == User.Groups[i].Name.ToLower())
                    {
                        User.Groups.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }
}

