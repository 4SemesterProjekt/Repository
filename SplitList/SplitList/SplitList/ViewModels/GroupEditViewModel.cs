using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ClientLibAPI;
using Prism.Commands;
using Prism.Mvvm;
using SplitList.Mapping;
using SplitList.Models;

namespace SplitList.ViewModels
{
    public class GroupEditViewModel : BindableBase
    {
        public GroupEditViewModel(int groupId)
        {
            _groupId = groupId;
        }

        private int _groupId;

        private Group _group;

        public Group Group
        {
            get => _group;
            set => SetProperty(ref _group, value);
        }

        private ICommand _onAppearing;

        public ICommand OnAppearing
        {
            get => _onAppearing ?? (_onAppearing = new DelegateCommand(OnAppearingExecute));
        }

        public async void OnAppearingExecute()
        {
            var result = await SerializerGroups.GetGroupByGroupId(Group.GroupId);
            Group = GroupMapper.GroupDtoToGroup(result);
        }
    }
}
