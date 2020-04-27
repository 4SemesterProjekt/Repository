using System;
using System.Collections.Generic;
using System.Text;
using Prism.Navigation.Xaml;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SplitList.Mapping;
using SplitList.Models;
using SplitList.Views;
using ClientLibAPI;
using Prism.Commands;
using Prism.Mvvm;

namespace SplitList.ViewModels
{
    public class RecipeViewModel : BindableBase
    {
        public RecipeViewModel()
        {
            Recipe = new Recipe();
           Ingredients = new ObservableCollection<Item>() { new Item("Salt", 1), new Item("Peber", 1), new Item("Meat", 4) };
        }

        #region Properties

        public Page Page { get; set; }

        private Recipe _recipe;

        public Recipe Recipe
        {
            get => _recipe; 
            set => SetProperty(ref _recipe, value);
        }

        private ObservableCollection<Item> _ingredients;

        public ObservableCollection<Item> Ingredients
        {
            get => _ingredients; 
            set => SetProperty(ref _ingredients, value);
        }



        #endregion

        #region Commands

        private ICommand _addToShoppingListCommand;

        public ICommand AddToShoppingListCommand => _addToShoppingListCommand ?? (_addToShoppingListCommand = new DelegateCommand(AddToShoppingListExecute));

        async void AddToShoppingListExecute()
        {
            // Get Pantry by group id
            Pantry pantry = new Pantry();
            pantry.Items.Add(new Item("Salt", 1));
            pantry.Items.Add(new Item("Peber", 1));
            pantry.Items.Add(new Item("Meat", 4));

            ObservableCollection<Item> notInPantry = new ObservableCollection<Item>();

            // Search Pantry for ingredients
            foreach (var ingredient in Ingredients)
            {
                bool isInPantry = false;
                foreach (var pantryItem in pantry.Items)
                {
                    if (pantryItem.Name.ToLower() == ingredient.Name.ToLower())
                        isInPantry = true;
                }
                if(!isInPantry)
                    notInPantry.Add(ingredient);
            }
            // if all ingredients are in pantry
            if (notInPantry.Count == 0)
                await Page.DisplayAlert("Alert", "All ingredients are already in your pantry","Okay");
            // DisplayMessage to user that all ingredients are already in pantry

            // If not all ingredients in pantry
            // DisplayMessage to user to select which shoppinglist to add ingredients to
            // Get shoppinglist by id
            // add items to shoppinglist
            // post shopping by id

        }

        private ICommand _removeFromPantryCommand;

        public ICommand RemoveFromPantryCommand => _removeFromPantryCommand ?? (_removeFromPantryCommand = new DelegateCommand(RemoveFromPantryExecute));

        public void RemoveFromPantryExecute()
        {
            // Get Pantry by group id
            // Search Pantry for ingredients
            // if all ingredients are in pantry 
            // Remove ingredients from pantry

            // if not all ingredients are in pantry
            // DisplayMessage inform user of missing ingredients
            // Prompt do you want to add them to a shoppinglist
        }

        #endregion
    }
}
