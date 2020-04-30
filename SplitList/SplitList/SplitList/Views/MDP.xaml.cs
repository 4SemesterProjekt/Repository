using System;
using System.Dynamic;
using SplitList.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SplitList.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MDP : MasterDetailPage
    {
        public MDP(int groupId, string userId)
        {
            InitializeComponent();
            _groupId = groupId;
            Master = new MenuView();
            Detail = new NavigationPage(new MultiShopListView(groupId, userId));
            MenuView.NavListView.ItemSelected += OnItemSelected;
            MenuView.NavListView1.ItemSelected += OnItemSelected;
        }

        private int _groupId;

        private string _userId;
        /// <summary>
        /// Navigates the page to the selected page from the menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //Typecast and null check with pattern matching
            if (e.SelectedItem is MenuViewItem item)
            {
                // Go to MultiShoppingListView
                if (item.TargetType == typeof(Views.MultiShopListView)) 
                    Detail = new NavigationPage(new MultiShopListView(_groupId, _userId));

                // Go to PantryView
                if(item.TargetType == typeof(Views.PantryView))
                    Detail = new NavigationPage( new PantryView(_groupId, _userId));
                
                //Go to RecipeView
                if(item.TargetType == typeof(Views.MultiRecipesView))
                    Detail = new NavigationPage(new MultiRecipesView(_groupId, _userId));

                // Go to UserView
                //if (item.TargetType == typeof(Views.UserView))
                    //Detail = new NavigationPage(new UserView(_groupId, _userId));

                    // Go to LoginView
                if (item.TargetType == typeof(Views.LoginView))
                    Application.Current.MainPage = new LoginView();
                
                // Clear selected item
                MenuView.NavListView.SelectedItem = null;
                MenuView.NavListView1.SelectedItem = null;
                // Hide the MenuView
                IsPresented = false;
            }
        }
    }
}