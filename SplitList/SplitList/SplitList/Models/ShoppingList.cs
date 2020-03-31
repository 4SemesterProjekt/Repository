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
    public class ShoppingList : INotifyPropertyChanged
    {
        public ShoppingList(string name)
        {
            Items = new ObservableCollection<Item>();
            Name = name;
        }

        public ShoppingList(ShoppingListDTO shoppingListDto)
        {
            ShoppingListDto = shoppingListDto;
            Items = new ObservableCollection<Item>(shoppingListDto.Items);
        }
        public ShoppingListDTO ShoppingListDto { get; set; }
        public string Name
        {
            get => ShoppingListDto.shoppingListName;
            set
            {
                ShoppingListDto.shoppingListName = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public ObservableCollection<Item> Items { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}