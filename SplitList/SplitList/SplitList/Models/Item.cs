using System;
using System.Collections.Generic;
using System.Text;
using Prism.Mvvm;

namespace SplitList.Models
{
    public class Item : BindableBase
    {
        public Item(string name, int amount, string category = "")
        {
            Name = name;
            Amount = (amount > 0 ? amount : 1);
            Category = category;
        }

        private string _name;
        private double _price;
        private int _amount;
        private string _category;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public double Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }
        public int Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }
        public string Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }
    }
}
