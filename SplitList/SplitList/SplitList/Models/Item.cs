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
            IsVisible = false;
            IsChecked = false;
        }
        public Item(string name, int amount, CheckBox CBox, string category = "")
        {
            Name = name;
            Amount = (amount > 0 ? amount : 1);
            Category = category;
            IsVisible = false;
            IsChecked = false;
        }
        public Item(string name, int amount, string category = "unknown")
        {
            Name = name;
            Amount = (amount > 0 ? amount : 1);
            Category = category;
            IsVisible = false;
            IsChecked = false;
        }

        #region Properties
        public int ItemId { get; set; }
        private string _name;
        private string _category;
        private int _amount;
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

        #region Commands

        private ICommand _incItemAmountCommand;

        public ICommand IncItemAmountCommand
        {
            get { return _incItemAmountCommand ?? (_incItemAmountCommand = new DelegateCommand(IncItemAmountExecute)); }
        }

        public void IncItemAmountExecute()
        {
            if (Amount < 99)
                Amount++;
        }

        private ICommand _decItemAmountCommand;

        public ICommand DecItemAmountCommand
        {
            get
            {
                return _decItemAmountCommand ?? (_decItemAmountCommand = new DelegateCommand(DecItemAmountCommandExecute));
            }
        }
        /// <summary>
        /// Decreases the amount of items in the GUI
        /// </summary>
        public void DecItemAmountCommandExecute()
        {
            if (Amount > 1)
                Amount--;
        }
        #endregion

    }
}
