using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Prism.Mvvm;
using Xamarin.Forms;

namespace SplitList.Models
{
    public class ShoppingList : BindableBase
    {
        public ShoppingList(string name)
        {
            Items = new ObservableCollection<Item>();
            Name = name;
        }

        public string Name { get; set; }
        public ObservableCollection<Item> Items { get; set; }

    }
}