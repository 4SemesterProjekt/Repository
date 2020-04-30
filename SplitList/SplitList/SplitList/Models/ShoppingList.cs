using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using ApiFormat;
using Prism.Mvvm;
using SplitList.Annotations;
using Xamarin.Forms;

namespace SplitList.Models
{
    public class ShoppingList : BindableBase
    {
        public ShoppingList()
        {
            Items = new ObservableCollection<Item>();
            Name = "";
            ShoppingListId = 0;
            GroupId = 1;
            IsVisible = false;
            IsChecked = false;
        }
        public ShoppingList(string name, int groupId)
        {
            Items = new ObservableCollection<Item>();
            Name = name;
            ShoppingListId = 0;
            GroupId = groupId;
            IsVisible = false;
            IsChecked = false;
        }

        public int ShoppingListId { get; set; }
        public int GroupId { get; set; }
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);

        }

        private bool _isVisible;
        private bool _isChecked;

        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);

        }

        public bool IsChecked
        {
            get => _isChecked;
            set => SetProperty(ref _isChecked, value);
        }

        private ObservableCollection<Item> _items;

        public ObservableCollection<Item> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

    }
}