using System;
using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace SplitList.Models
{
    public class Pantry : BindableBase
    {
        public Pantry()
        {
            Items = new ObservableCollection<Item>();
            Name = "";
            GroupName = "";
            PantryId = 0;
            GroupId = 1;
        }

        public Pantry(string name, string groupName, int groupId)
        {
            Items = new ObservableCollection<Item>();
            Name = name;
            GroupName = groupName;
            PantryId = 0;
            GroupId = groupId;
        }

        public int GroupId { get; set; }
        public int PantryId { get; set; }
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _groupName;
        public string GroupName
        {
            get => _groupName;
            set => SetProperty(ref _groupName, value);
        }

        private ObservableCollection<Item> _items;

        public ObservableCollection<Item> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

    }
}