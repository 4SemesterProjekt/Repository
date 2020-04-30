using SplitList.Models;
using SplitList.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SplitList.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShoppingListView : ContentPage
    {
        public ShoppingListView(int shoppingListId, int groupId, string userId)
        {
            InitializeComponent();
            BindingContext = new ShoppingListViewModel(shoppingListId, Navigation, this, groupId, userId);
        }

    }
}