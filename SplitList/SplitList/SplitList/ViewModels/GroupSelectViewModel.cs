using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using ApiFormat.Group;
using ApiFormat.User;
using ClientLibAPI;
using Prism.Commands;
using Prism.Mvvm;
using SplitList.Models;
using SplitList.Views;
using Xamarin.Forms;

namespace SplitList.ViewModels
{
    class GroupSelectViewModel : BaseViewModel
    {
        public GroupSelectViewModel(INavigation nav, Page page, int groupId, string userId) : base(nav, page, groupId, userId)
        {
            Groups = new ObservableCollection<Group>();
        }

        private ObservableCollection<Group> _groups;

        public ObservableCollection<Group> Groups
        {
            get => _groups;
            set => SetProperty(ref _groups, value);
        }

        private ICommand _itemTappedCommand;

        public ICommand ItemTappedCommand
        {
            get => _itemTappedCommand ?? (_itemTappedCommand = new DelegateCommand<object>(ItemTappedExecute));
        }

        public void ItemTappedExecute(object group)
        {
            if(group is Group g)
                Application.Current.MainPage = new MDP(g.GroupId, UserId);
        }

        public override async void OnAppearingExecute()
        {
            var returnedUser = await SerializerUser.GetUserById(UserId);
            User user = mapper.Map<User>(returnedUser);
            Groups = user.Groups;

            if (Groups.Count == 0)
            {
                var result = await Application.Current.MainPage.DisplayPromptAsync("No group found", "Please input a name for your group", "Create Group");
                Group newGroup = new Group() {Name = result};
                newGroup.Users.Add(user);
                var returnedGroup = await SerializerGroup.CreateGroup(mapper.Map<GroupDTO>(newGroup));
                Groups.Add(mapper.Map<Group>(returnedGroup));
                
            }
        }
    }
}
