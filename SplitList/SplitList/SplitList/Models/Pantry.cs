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
            PantryId = 0;
            GroupId = 1;
        }

        public Pantry(string name, int groupId)
        {
            Items = new ObservableCollection<Item>();
            Name = name;
            PantryId = 0;
            GroupId = groupId;
        }

        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int PantryId { get; set; }
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private ObservableCollection<Item> _items;

        public ObservableCollection<Item> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

    }
}