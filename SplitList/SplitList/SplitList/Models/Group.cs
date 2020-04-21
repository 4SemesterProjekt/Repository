using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Prism.Mvvm;

namespace SplitList.Models
{
    public class Group : BindableBase
    {
        private string _name;
        private int _groupId;
        private string _groupOwnerId;
        private ObservableCollection<User> _users;

        public string GroupOwnerId
        {
            get => _groupOwnerId;
            set => SetProperty(ref _groupOwnerId, value);

        }

        public ObservableCollection<User> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);

        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);

        }

        public int GroupId
        {
            get => _groupId;
            set => SetProperty(ref _groupId, value);
        }

    }
}
