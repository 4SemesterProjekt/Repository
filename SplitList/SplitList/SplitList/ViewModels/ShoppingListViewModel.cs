﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Input;
using ApiFormat;
using ClientLibAPI;
using Prism.Commands;
using Prism.Mvvm;
using SplitList.Mapping;
using SplitList.Models;
using Xamarin.Forms;

namespace SplitList.ViewModels
{
    public class ShoppingListViewModel : BindableBase
    {
        public ShoppingListViewModel()
        {
            ShoppingList = new ShoppingList();
        }

        
        #region Properties

        private ShoppingList _shoppingList;

        public ShoppingList ShoppingList
        {
            get => _shoppingList;
            set => SetProperty(ref _shoppingList, value);
        }

        private Item _currentItem;

        public Item CurrentItem
        {
            get => _currentItem;
            set => SetProperty(ref _currentItem, value);
        }

        #endregion

        #region Commands

        private ICommand _incItemAmountCommand;

        public ICommand IncItemAmountCommand
        {
            get
            {
                return _incItemAmountCommand ?? (_incItemAmountCommand = new DelegateCommand(IncItemAmountCommandExecute));

            }
        }

        public void IncItemAmountCommandExecute()
        {
            if (CurrentItem != null)
            {
                if (CurrentItem.Amount < 99)
                    CurrentItem.Amount++;
            }
        }

        private ICommand _decItemAmountCommand;

        public ICommand DecItemAmountCommand
        {
            get
            {
                return _decItemAmountCommand ?? (_decItemAmountCommand = new DelegateCommand(DecItemAmountCommandExecute));

            }
        }

        public void DecItemAmountCommandExecute()
        {
            if (CurrentItem != null)
            {
                if (CurrentItem.Amount > 1)
                    CurrentItem.Amount--;
            }
        }


        private ICommand _addItemToListCommand;
        public ICommand AddItemToListCommand
        {
            get
            {
                return _addItemToListCommand ?? (_addItemToListCommand = new DelegateCommand<object>(AddItemToListCommandExecute));
            }
        }

        public void AddItemToListCommandExecute(object obj)
        {
           ShoppingList.Items.Add(new Item("",1,obj as CheckBox));

        }

        private ICommand _deleteItemCommand;

        public ICommand DeleteItemCommand
        {
            get { return _deleteItemCommand ?? (_deleteItemCommand = new DelegateCommand(DeleteItemExecute)); }
        }

        public void DeleteItemExecute()
        {
            ShoppingList.Items.Remove(CurrentItem);
        }

        #endregion
    }
}
