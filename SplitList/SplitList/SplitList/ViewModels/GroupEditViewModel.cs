using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
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

        void HandleUserRemoveEvent(object source, EventArgs e)
        {

        }

        
    }
}
