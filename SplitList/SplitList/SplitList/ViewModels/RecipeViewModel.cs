using System;
using System.Collections.Generic;
using System.Text;
using Prism.Navigation.Xaml;
using Xamarin.Forms;
using System.Collections.ObjectModel;
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
        }

        #region Properties

        public Page Page { get; set; }

        private Recipe _recipe;

        public Recipe Recipe
        {
            get => _recipe; 
            set => SetProperty(ref _recipe, value);
        }

        private List<Item> _ingredients;

        public List<Item> Ingredients
        {
            get => _ingredients; 
            set => SetProperty(ref _ingredients, value);
        }



        #endregion
    }
}
