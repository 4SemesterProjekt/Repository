using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using ApiFormat.Group;
using ClientLibAPI;
using Prism.Commands;
using Prism.Mvvm;
using SplitList.Models;
using Xamarin.Forms;

namespace SplitList.ViewModels
{
    public class GroupEditViewModel : BaseViewModel
    {

        public GroupEditViewModel(INavigation nav, Page page, int groupId, string userId) : base(nav, page, groupId, userId)
        {
            Group = new Group();
            Group.UserRemoveEvent += HandleUserRemoveEvent;
        }

        private Group _group;

        public Group Group
        {
            get => _group;
            set => SetProperty(ref _group, value);
        }

        public override async void OnAppearingExecute()
        {
            var result = await SerializerGroup.GetGroupById(GroupId);
            Group = mapper.Map<Group>(result);
        }

        public override async void OnDisappearingExecute()
        {
            var returnedGroup = await SerializerGroup.UpdateGroup(mapper.Map<GroupDTO>(Group));
        }

        public async void HandleUserRemoveEvent(object source, EventArgs e)
        {
            var result = await Page.DisplayAlert("Alert", "Are you sure you want to remove the user from the group", "Yes",
                "Cancel");
            if (!result) return;
            if (source is User userIn)
            {
                for (int i = 0; i < Group.Users.Count; i++)
                {
                    if (Group.Users[i].Id == userIn.Id)
                    {
                        Group.Users.RemoveAt(i);
                        break;
                    }
                }
            }

        }

        private ICommand _addUserCommand;

        public ICommand AddUserCommand => _addUserCommand ?? (_addUserCommand = new DelegateCommand(AddUserExecute));

        public async void AddUserExecute()
        {
            // Run multiple times if email not found
            while(true)
            {
                // Prompt user for email of person they want to add
                var email = await Page.DisplayPromptAsync("Add Person to group", "Input the email of the user you want to add", "Add");

                // Check if user is already in group
                bool isInGroup = false;
                foreach (var groupUser in Group.Users)
                {
                    // Ignore case sensitivity 
                    if (groupUser.Email.ToLower() == email.ToLower())
                    {
                        isInGroup = true;
                        break;
                    }
                }

                if (isInGroup)
                {
                    // If in group prompt user
                    await Page.DisplayAlert("Whoops", "User is already in your group", "Okay");
                    break;
                }
                else
                {
                    // If not in group search for user
                    var result = mapper.Map<User>(await SerializerUser.GetUserByEmail(email));
                    if (result != null)
                    {
                        // Add user to group
                        Group.Users.Add(result);
                        await Page.DisplayAlert("Success", "User has been added to your group", "Done");
                        break;
                    }
                    else
                    {
                        // Prompt User that action failed and if they want to try again
                        var tryAgain = await Page.DisplayAlert("404",
                            "No user found with that email, do you want to try another email?", "Yes", "No");
                        if (tryAgain == false)
                            break;
                    }
                }
            }
        }
    }
}
