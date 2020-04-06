using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using ApiFormat;
using Prism.Commands;
using Prism.Mvvm;
using SplitList.Annotations;
using Xamarin.Forms;

namespace SplitList.Models
{
    public class Item : BindableBase
    {
        public Item()
        {
            Name = "";
            Amount = 1;
        }
        public Item(string name, int amount, CheckBox CBox, string category = "")
        {
            Name = name;
            Amount = (amount > 0 ? amount : 1);
            Category = category;
        }

        #region Properties

        public CheckBox CheckBox { get; set; }
        public int ItemId { get; set; }
        private string _name;
        private string _category;
        private int _amount;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);

        }

        public string Category
        {
            get => _category;
            set => SetProperty(ref _category, value);

        }

        public int Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);

        }

        #endregion

    }
}
