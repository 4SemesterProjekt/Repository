﻿using System.Windows.Input;
using ApiFormat.Pantry;
using ApiFormat.ShoppingList;
using ClientLibAPI;
using Prism.Commands;
using SplitList.Models;
using Xamarin.Forms;

namespace SplitList.ViewModels
{
    public class ShoppingListViewModel : BaseViewModel
    {
        public ShoppingListViewModel(int shoppingListId, INavigation nav, Page page, int groupId, string userId) : base(nav, page, groupId, userId)
        {
            ShoppingList = new ShoppingList(){ShoppingListId =  shoppingListId};
        }

        #region Properties
        private bool _addBtnIsVisible = true;
        private bool _addBtnIsEnabled = true;

        public bool AddBtnIsVisible
        {
            get => _addBtnIsVisible;
            set => SetProperty(ref _addBtnIsVisible, value);

        }

        public bool AddBtnIsEnabled
        {
            get => _addBtnIsEnabled;
            set => SetProperty(ref _addBtnIsEnabled, value);
        }
        private bool _confirmBtnIsVisible;
        private bool _confirmBtnIsEnabled;

        public bool ConfirmBtnIsVisible
        {
            get => _confirmBtnIsVisible;
            set => SetProperty(ref _confirmBtnIsVisible, value);

        }

        public bool ConfirmBtnIsEnabled
        {
            get => _confirmBtnIsEnabled;
            set => SetProperty(ref _confirmBtnIsEnabled, value);
        }

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
                return _addItemToListCommand ?? (_addItemToListCommand = new DelegateCommand(AddItemToListCommandExecute));
            }
        }
        /// <summary>
        /// Ads an item to the shown shopping list defaults empty name and amount of 1, for the user to edit afterwards
        /// </summary>
        /// <param name="obj">Is the checkbox used for checks</param>
        public void AddItemToListCommandExecute()
        {
           ShoppingList.Items.Add(new Item("",1));

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

        private ICommand _startShoppingCommand;

        public ICommand StartShoppingCommand
        {
            get => _startShoppingCommand ?? (_startShoppingCommand = new DelegateCommand(StartShoppingExecute));
        }

        public void StartShoppingExecute()
        {
            foreach (var shoppingListItem in ShoppingList.Items)
            {
                shoppingListItem.IsVisible = true;
            }

            AddBtnIsEnabled = false;
            AddBtnIsVisible = false;
            ConfirmBtnIsEnabled = true;
            ConfirmBtnIsVisible = true;
        }

        private ICommand _confirmBtnCommand;

        public ICommand ConfirmBtnCommand => _confirmBtnCommand ?? (_confirmBtnCommand = new DelegateCommand(ConfirmBtnExecute));

        public async void ConfirmBtnExecute()
        {
            // Get pantry by groupID
            Group group = mapper.Map<Group>(await SerializerGroup.GetGroupById(GroupId));
            var pantry = mapper.Map<Pantry>(await SerializerPantry.GetPantryById(group.Pantry.PantryId));
            // Add checked items to pantry
            foreach (var shoppingListItem in ShoppingList.Items)
            {
                if (shoppingListItem.IsChecked)
                {
                    bool isInPantry = false;
                    foreach (var pantryItem in pantry.Items)
                    {
                        if (pantryItem.Name.ToLower() == shoppingListItem.Name.ToLower())
                        {
                            pantryItem.Amount += shoppingListItem.Amount;
                            isInPantry = true;
                            break;
                        }
                    }
                    if(!isInPantry)
                        pantry.Items.Add(shoppingListItem);
                }
            }
            // Post pantry by group ID
            var returnedPantry = await SerializerPantry.UpdatePantry(mapper.Map<PantryDTO>(pantry));

            for (int i = ShoppingList.Items.Count - 1; i >= 0; i--)
            {
                if (ShoppingList.Items[i].IsChecked)
                    ShoppingList.Items.Remove(ShoppingList.Items[i]);

            }

            foreach (var shoppingListItem in ShoppingList.Items)
            {
                shoppingListItem.IsVisible = false;
                shoppingListItem.IsChecked = false;
            }

            AddBtnIsVisible = true;
            AddBtnIsEnabled = true;
            ConfirmBtnIsEnabled = false;
            ConfirmBtnIsVisible = false;
        }

        public override async void OnAppearingExecute()
        {
            var result = await SerializerShoppingList.GetShoppingListById(ShoppingList.ShoppingListId);
            ShoppingList = mapper.Map<ShoppingList>(result);
        }

        public override async void OnDisappearingExecute()
        {
            foreach (var shoppingListItem in ShoppingList.Items)
            {
                shoppingListItem.IsVisible = false;
                shoppingListItem.IsChecked = false;
            }
            AddBtnIsVisible = true;
            AddBtnIsEnabled = true;
            ConfirmBtnIsEnabled = false;
            ConfirmBtnIsVisible = false;
            var result = await SerializerShoppingList.UpdateShoppingList(mapper.Map<ShoppingListDTO>(ShoppingList));
        }
        #endregion
    }
}
