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
        public ShoppingList()
        {
            Items = new ObservableCollection<Item>();
        }

        public ObservableCollection<Item> Items { get; set; }
    }
}