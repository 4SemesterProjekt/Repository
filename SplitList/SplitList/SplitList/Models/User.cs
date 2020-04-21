using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Xamarin.Forms;

namespace SplitList.Models
{
    public class User : BindableBase
    {
        public User()
        {
            Groups = new ObservableCollection<Group>();
        }

        private string _name;
        private ObservableCollection<Group> _groups;
        private string _id;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);

        }

        public ObservableCollection<Group> Groups
        {
            get => _groups;
            set => SetProperty(ref _groups, value);

        }

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);

        }

    }
}
