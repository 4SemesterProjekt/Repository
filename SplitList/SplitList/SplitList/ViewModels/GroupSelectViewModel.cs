using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using SplitList.Models;
using SplitList.Views;
using Xamarin.Forms;

namespace SplitList.ViewModels
{
    class GroupSelectViewModel : BindableBase
    {
        public GroupSelectViewModel()
        {
            Groups = new ObservableCollection<Group>();
            Groups.Add(new Group(){Name = "Familien Splitlist", GroupId = 1});
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
            var g = group as Group;
            Application.Current.MainPage = new MDP(g.GroupId);
        }

        private ICommand _onAppearing;

        public ICommand OnAppearing
        {
            get => _onAppearing ?? (_onAppearing = new DelegateCommand(OnAppearingExecute));
        }

        public async void OnAppearingExecute()
        {
            if (Groups.Count == 0)
            {
                var result = await Application.Current.MainPage.DisplayPromptAsync("No group found", "Please input a name for your group", "Create Group");
                Groups.Add(new Group(){GroupId = 0, Name = result});
            }
        }
    }
}
