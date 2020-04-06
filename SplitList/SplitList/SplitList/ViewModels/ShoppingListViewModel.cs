using System;
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
using SplitList.Views;
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

        public Page Page { get; set; }
        private bool deleteState = false;

        private ShoppingList _shoppingList;

        public ShoppingList ShoppingList
        {
            get => _shoppingList;
            set => SetProperty(ref _shoppingList, value);
        }
        
        #endregion

        #region Commands

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

        public async void DeleteItemExecute()
        {
            if (!deleteState)
            {
                foreach (var shoppingListItem in ShoppingList.Items)
                {
                    shoppingListItem.IsVisible = true;
                }

                deleteState = true;
            }else if(deleteState)
            {
                bool isAnyChecked = false;
                foreach (var shoppingListItem in ShoppingList.Items)
                {
                    if (shoppingListItem.IsChecked)
                    {
                        isAnyChecked = true;
                        break;
                    }
                }

                if (isAnyChecked)
                {
                    var result = await Page.DisplayAlert("Advarsel", "Er du sikker på slette disse items?", "Ja", "Nej");
                    if (result)
                    {
                        for (int i = ShoppingList.Items.Count - 1; i >= 0; i--)
                        {
                            if (ShoppingList.Items[i].IsChecked)
                                ShoppingList.Items.Remove(ShoppingList.Items[i]);
                        }
                    }
                }
                foreach (var shoppingListItem in ShoppingList.Items)
                {
                    shoppingListItem.IsChecked = false;
                    shoppingListItem.IsVisible = false;
                }
                deleteState = false;
            }
            
        }

        
        #endregion
    }
}
