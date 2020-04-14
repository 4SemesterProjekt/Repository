using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Prism.Mvvm;
using SplitList.Models;
using Xamarin.Forms;

namespace SplitList.ViewModels
{
    class MultiRecipesViewModel : BindableBase
    {
        public MultiRecipesViewModel(INavigation navigation, Page page)
        {
            _page = page;
            Navigation = navigation;
            Recipes = new ObservableCollection<Recipe>();

            Recipes.Add(new Recipe("Spegetti bolo", "Lækkereste easy mam", "Find selv ud af det."));
            Recipes.Add(new Recipe("Spegetti carba", "Lækkereste easy mum", "Find selv ud af det."));
            Recipes.Add(new Recipe("Spegetti bacon", "Lækkereste easy nam", "Find selv ud af det."));
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
    }
}
