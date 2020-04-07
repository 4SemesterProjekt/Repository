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
        /// <summary>
        /// Ads an item to the shown shopping list defaults empty name and amount of 1, for the user to edit afterwards
        /// </summary>
        /// <param name="obj">Is the checkbox used for checks</param>
        public void AddItemToListCommandExecute(object obj)
        {
           ShoppingList.Items.Add(new Item("",1,obj as CheckBox));

        }

        private ICommand _deleteItemCommand;

        public ICommand DeleteItemCommand
        {
            get { return _deleteItemCommand ?? (_deleteItemCommand = new DelegateCommand(DeleteItemExecute)); }
        }

        /// <summary>
        /// On first press shows a checkbox next to each item
        /// On second press, if any checkbox is checked prompts the user to confirm deletions, if chosen any selected Items will be deleted
        /// </summary>
        public async void DeleteItemExecute()
        {
            //On first press
            if (!deleteState) 
            {
                foreach (var shoppingListItem in ShoppingList.Items)
                {
                    shoppingListItem.IsVisible = true;
                }

                deleteState = true;
            }
            //On second press
            else if (deleteState) 
            {
                bool isAnyChecked = false;
                //Runs the list through to see if an item is checked, avoids prompting the user when no deletion is intented
                foreach (var shoppingListItem in ShoppingList.Items)
                {
                    if (shoppingListItem.IsChecked)
                    {
                        isAnyChecked = true;
                        break;
                    }
                }
                //If an item is checked prompts the user to confirm their deletions
                if (isAnyChecked)
                {
                    var result = await Page.DisplayAlert("Warning", "Are you sure that you want to delete the selected items?", "Yes", "No");
                    if (result)
                    {
                        for (int i = ShoppingList.Items.Count - 1; i >= 0; i--)
                        {
                            //Does not update itself on the database, it is done when the page is closed
                            if (ShoppingList.Items[i].IsChecked)
                                ShoppingList.Items.Remove(ShoppingList.Items[i]);
                        }
                    }
                }
                //Hides all the checkboxes again
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
