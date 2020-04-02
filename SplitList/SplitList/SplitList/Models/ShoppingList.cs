﻿using System;
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
        }
        public ShoppingList(string name)
        {
            Items = new ObservableCollection<Item>();
            Name = name;
        }

        public int ShoppingListId { get; set; }
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