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
    public class MultiRecipesViewModel : BaseViewModel
    {
        public MultiRecipesViewModel(INavigation nav, Page page, int groupId) : base(nav, page, groupId)
        {
            Recipes = new ObservableCollection<Recipe>();

            Recipes.Add(new Recipe("Spegetti bolo", "Det her er min allerbedste opskrift på chili con carne – og den falder bare altid i god jord, hvad end vi laver den til hverdag eller når vi har gæster. Jeg laver gerne vores chili con carne i en mild børnefamilievenlig version. Der er stadig masser power på smagen, men den er ikke spor stærk – så alle kan være med! Man kan sagtens tilføje mere chili for at få den slags lækre chili der giver varme, jeg er personligt vild med chili con carne uanset om det er den stærke eller den mere børnevenlige udgave.", "Find selv ud af det. Eller søg på google i dont give a fuck. lel omega lul. Stfu worly din dumme cunt xD. I have over 258 confirmed kill and i know where you live. Tilsæt spidskommen, kanel, koriander, hvidløg og chili. Rør rundt i et par minutter, skru lidt ned for varmen og tilsæt de hakkede løg. Rør rundt til løgene er gennemsigtige og klare, og skru derefter op for varmen, tilsæt oksekød og brun kødet godt af under omrøring. Tilsæt flåede/hakkede tomater, soltørrede tomater og kalvefond/boullion. Læg låg på og lad det simre i en halv times tid. Smag saucen til og hvis den skal have mere power, så tilføj chili eller cayennepeber. Hvis saucen derimod er blevet for stærk, så vent med at gøre noget. Bønnerne og chokoladen runder den af tilsidst. Tilsæt bønner og varm  dem godt igennem i 5 minutter. Tag gryden af varmen og rør chokoladen i. Start med halvdelen og smag til med resten. Smag tilsidst til med salt og peber, og hvis chilien skulle være blevet for stærk, så kan det klares med en tsk rørsukker. Ellers må man spise lidt ekstra creme fraiche til, det dulmer smagsløgene. Server med nachos, ris, godt brød, creme fraiche og gratiner evt med cheddarost"));
            Recipes.Add(new Recipe("Spegetti carba", "Lækkereste easy mum", "Find selv ud af det."));
            Recipes.Add(new Recipe("Spegetti bacon", "Lækkereste easy nam", "Find selv ud af det."));
        }

        #region Properties

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

