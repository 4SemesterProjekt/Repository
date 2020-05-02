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
    { public RecipeViewModel(int recipeId, INavigation nav, Page page, int groupId, string userId) : base(nav, page, groupId, userId)
        {
            Recipe = new Recipe("Spegetti bolo", "Det her er min allerbedste opskrift på chili con carne – og den falder bare altid i god jord, hvad end vi laver den til hverdag eller når vi har gæster. Jeg laver gerne vores chili con carne i en mild børnefamilievenlig version. Der er stadig masser power på smagen, men den er ikke spor stærk – så alle kan være med! Man kan sagtens tilføje mere chili for at få den slags lækre chili der giver varme, jeg er personligt vild med chili con carne uanset om det er den stærke eller den mere børnevenlige udgave.", "Find selv ud af det. Eller søg på google i dont give a fuck. lel omega lul. Stfu worly din dumme cunt xD. I have over 258 confirmed kill and i know where you live. Tilsæt spidskommen, kanel, koriander, hvidløg og chili. Rør rundt i et par minutter, skru lidt ned for varmen og tilsæt de hakkede løg. Rør rundt til løgene er gennemsigtige og klare, og skru derefter op for varmen, tilsæt oksekød og brun kødet godt af under omrøring. Tilsæt flåede/hakkede tomater, soltørrede tomater og kalvefond/boullion. Læg låg på og lad det simre i en halv times tid. Smag saucen til og hvis den skal have mere power, så tilføj chili eller cayennepeber. Hvis saucen derimod er blevet for stærk, så vent med at gøre noget. Bønnerne og chokoladen runder den af tilsidst. Tilsæt bønner og varm  dem godt igennem i 5 minutter. Tag gryden af varmen og rør chokoladen i. Start med halvdelen og smag til med resten. Smag tilsidst til med salt og peber, og hvis chilien skulle være blevet for stærk, så kan det klares med en tsk rørsukker. Ellers må man spise lidt ekstra creme fraiche til, det dulmer smagsløgene. Server med nachos, ris, godt brød, creme fraiche og gratiner evt med cheddarost");
            Recipe.Ingredients = new ObservableCollection<Item>() { new Item("Salt", 1), new Item("Peber", 1), new Item("Meat", 4) };
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
                foreach (var ingredient in Recipe.Ingredients)
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
            bool allIsInPantry = false;
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
                if (result)
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
                    foreach (var ingredient in Recipe.Ingredients)
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
        }

        #endregion

        
    }
}
