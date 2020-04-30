using SplitList.Models;
using SplitList.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SplitList.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShoppingListView : ContentPage
    {
        public ShoppingListView(ShoppingList shoppingList, int groupId, string userId)
        {
            InitializeComponent();
            BindingContext = new ShoppingListViewModel(shoppingList, Navigation, this, groupId, userId);
        }

    }
}