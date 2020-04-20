using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using SplitList.Models;
using SplitList.Views;
using Xamarin.Forms;

namespace SplitList.ViewModels
{
    public class MultiRecipesViewModel : BindableBase
    {
        public MultiRecipesViewModel(INavigation navigation, Page page)
        {
            _page = page;
            Navigation = navigation;
            Recipes = new ObservableCollection<Recipe>();

            Recipes.Add(new Recipe(1,"Spegetti bolo", "Lækkereste easy mam", "Find selv ud af det."));
            Recipes.Add(new Recipe(2,"Spegetti carba", "Lækkereste easy mum", "Find selv ud af det."));
            Recipes.Add(new Recipe(3,"Spegetti bacon", "Lækkereste easy nam", "Find selv ud af det."));
        }

        #region Properties

        private Page _page;

        private INavigation Navigation { get; set; }

        private ObservableCollection<Recipe> _recipes;

        public ObservableCollection<Recipe> Recipes
        {
            get => _recipes; 
            set => SetProperty(ref _recipes, value);
        }

        private Recipe _currenRecipe;

        public Recipe CurrentRecipe
        {
            get => _currenRecipe;
            set => SetProperty(ref _currenRecipe, value);
        }


        #endregion

        #region Commands

        private ICommand _itemTappedCommand;

        public ICommand ItemTappedCommand
        {
            get => _itemTappedCommand ?? (_itemTappedCommand = new DelegateCommand(OpenRecipeExecute));
        }

        async void OpenRecipeExecute()
        {
            await Navigation.PushAsync(new RecipeView(CurrentRecipe));
        }


        #endregion
    }
}

