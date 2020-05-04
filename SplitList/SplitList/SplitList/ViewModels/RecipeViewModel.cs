using System.Collections.Generic;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ApiFormat.Pantry;
using ApiFormat.ShoppingList;
using ClientLibAPI;
using SplitList.Models;
using Prism.Commands;

namespace SplitList.ViewModels
{
    public class RecipeViewModel : BaseViewModel
    { public RecipeViewModel(Recipe recipe, INavigation nav, Page page, int groupId, string userId) : base(nav, page, groupId, userId)
        {
            Recipe = new Recipe();
            Recipe = recipe;

        }

        #region Properties
        
        private Recipe _recipe;

        public Recipe Recipe
        {
            get => _recipe; 
            set => SetProperty(ref _recipe, value);
        }




        #endregion

        #region Commands

        private ICommand _addToShoppingListCommand;

        public ICommand AddToShoppingListCommand => _addToShoppingListCommand ?? (_addToShoppingListCommand = new DelegateCommand(AddToShoppingListExecute));

        async void AddToShoppingListExecute()
        {
            // Get Pantry by group id
            var group = mapper.Map<Group>(await SerializerGroup.GetGroupById(GroupId));
            var pantry = mapper.Map<Pantry>(await SerializerPantry.GetPantryById(group.Pantry.PantryId));

            List<Item> notInPantry = new List<Item>();

            // Search Pantry for ingredients
            foreach (var ingredient in Recipe.Ingredients)
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
            {
                await Page.DisplayAlert("Alert", "All ingredients are already in your pantry", "Okay");
            }
            // if not all ingredients are in pantry
            else
            {
                // Get shoppinglist by group id
                ObservableCollection<ShoppingList> shoppingLists = group.ShoppingLists;
                string[] options = new string[shoppingLists.Count];
                for (int i = 0; i < shoppingLists.Count; i++)
                {
                    options[i] = shoppingLists[i].Name;
                }
                
                // DisplayMessage to user to select which shoppinglist to add ingredients to
                string action;
                action = await Page.DisplayActionSheet("Choose shoppinglist", "Cancel", null, options);

                // Get shoppinglist by id
                var shoppingList = new ShoppingList();
                for (int i = 0; i < shoppingLists.Count; i++)
                {
                    if (shoppingLists[i].Name.ToLower() == action.ToLower())
                    {
                        shoppingList = mapper.Map<ShoppingList>(await SerializerShoppingList.GetShoppingListById(shoppingLists[i].ShoppingListId));
                        break;
                    }
                }
                // add items to shoppinglist
                foreach (var ingredient in notInPantry)
                {
                    bool isInShoppingList = false;
                    foreach (var shoppingListItem in shoppingList.Items)
                    {
                        if (ingredient.Name.ToLower() == shoppingListItem.Name.ToLower())
                        {
                            shoppingListItem.Amount += ingredient.Amount;
                            isInShoppingList = true;
                            break;
                        }
                    }
                    if(!isInShoppingList)
                        shoppingList.Items.Add(ingredient);
                }
                // post shopping by id
                var returnedShoppingList = await SerializerShoppingList.UpdateShoppingList(mapper.Map<ShoppingListDTO>(shoppingList));

            }
            
        }

        private ICommand _removeFromPantryCommand;

        public ICommand RemoveFromPantryCommand => _removeFromPantryCommand ?? (_removeFromPantryCommand = new DelegateCommand(RemoveFromPantryExecute));

        async void RemoveFromPantryExecute()
        {
            // Get Pantry by group id
            var group = mapper.Map<Group>(await SerializerGroup.GetGroupById(GroupId));
            var pantry = mapper.Map<Pantry>(await SerializerPantry.GetPantryById(group.Pantry.PantryId));

            // Search Pantry for ingredients
            List<Item> notInPantry = new List<Item>();

            foreach (var ingredient in Recipe.Ingredients)
            {
                bool isInPantry = false;
                foreach (var pantryItem in pantry.Items)
                {
                    if (pantryItem.Name.ToLower() == ingredient.Name.ToLower())
                    {
                        isInPantry = true;
                        break;
                    }
                }
                if (!isInPantry)
                    notInPantry.Add(ingredient);
            }
            // if all ingredients are in pantry 
            if (notInPantry.Count == 0)
            {
                // Remove ingredients from pantry
                for (int i = pantry.Items.Count - 1; i >= 0; i--)
                {
                    if (Recipe.Ingredients[i].Name.ToLower() == pantry.Items[i].Name.ToLower())
                        pantry.Items.Remove(pantry.Items[i]);
                }
                //Post Pantry to Database
                var returnedPantry = await SerializerPantry.UpdatePantry(mapper.Map<PantryDTO>(pantry));
            }
            // if not all ingredients are in pantry
            else
            {
                // Prompt do you want to add them to a shoppinglist
                var result =await Page.DisplayAlert("Not all items found in pantry", "Do you want to add them to a shoppinglist?",
                    "Yes", "No");
                if (!result) return;
                // Get shoppinglist by group id
                ObservableCollection<ShoppingList> shoppingLists = @group.ShoppingLists;
                string[] options = new string[shoppingLists.Count];
                for (int i = 0; i < shoppingLists.Count; i++)
                {
                    options[i] = shoppingLists[i].Name;
                }

                // DisplayMessage to user to select which shoppinglist to add ingredients to
                string action;
                action = await Page.DisplayActionSheet("Choose shoppinglist", "Cancel", null, options);

                // Get shoppinglist by id
                var shoppingList = new ShoppingList();
                for (int i = 0; i < shoppingLists.Count; i++)
                {
                    if (shoppingLists[i].Name.ToLower() == action.ToLower())
                    {
                        shoppingList = mapper.Map<ShoppingList>(await SerializerShoppingList.GetShoppingListById(shoppingLists[i].ShoppingListId));
                        break;
                    }
                }
                // add items to shoppinglist
                foreach (var ingredient in notInPantry)
                {
                    bool isInShoppingList = false;
                    foreach (var shoppingListItem in shoppingList.Items)
                    {
                        if (ingredient.Name.ToLower() == shoppingListItem.Name.ToLower())
                        {
                            shoppingListItem.Amount += ingredient.Amount;
                            isInShoppingList = true;
                            break;
                        }
                    }
                    if (!isInShoppingList)
                        shoppingList.Items.Add(ingredient);
                }
                // post shopping by id
                var returnedShoppingList = await SerializerShoppingList.UpdateShoppingList(mapper.Map<ShoppingListDTO>(shoppingList));
            }
        }

        #endregion

        
    }
}
