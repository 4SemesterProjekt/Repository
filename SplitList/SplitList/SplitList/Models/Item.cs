using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using ApiFormat;
using Prism.Mvvm;
using SplitList.Annotations;
using Xamarin.Forms;

namespace SplitList.Models
{
    public class Item : INotifyPropertyChanged
    {
        public Item(string name, int amount, string category = "")
        { 
            ItemDto = new ItemDTO();
            Name = name;
            Amount = (amount > 0 ? amount : 1);
            Category = category;
        }

        public Item(ItemDTO itemDto)
        {
            ItemDto = itemDto;
        }

        public ItemDTO ItemDto { get; set; }

        public string Name
        {
            get => ItemDto.Name;
            set
            {
                ItemDto.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public int Amount
        {
            get => ItemDto.Amount;
            set
            {
                ItemDto.Amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }
        public string Category
        {
            get => ItemDto.Type;
            set
            {
                ItemDto.Type = value;
                OnPropertyChanged(nameof(Category));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
