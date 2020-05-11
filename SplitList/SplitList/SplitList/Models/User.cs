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
        public event EventHandler RemoveUserEvent;
        public User()
        {
            Groups = new ObservableCollection<Group>();
        }

        private string _name;
        private ObservableCollection<Group> _groups;
        private string _id;
        private int _modelId;
        public string Email { get; set; }

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

        public int ModelId
        {
            get => _modelId;
            set => SetProperty(ref _modelId, value);
        }

        private ICommand _userRemoveCommand;

        public ICommand UserRemoveCommand => _userRemoveCommand ?? (_userRemoveCommand = new DelegateCommand(RemoveUserExecute));

        public void RemoveUserExecute()
        {
            RemoveUserEvent?.Invoke(this , EventArgs.Empty);
        }
    }
}
