using SplitList.Models;
using SplitList.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SplitList.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipeView : ContentPage
    {
        public RecipeView(Recipe recipe, int groupId, string userId)
        {
            InitializeComponent();
            BindingContext = new RecipeViewModel(Navigation, this, groupId, userId);
        }
    }
}