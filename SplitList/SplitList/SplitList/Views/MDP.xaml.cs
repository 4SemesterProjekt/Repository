using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplitList.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SplitList.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MDP : MasterDetailPage
    {
        public MDP(int groupId)
        {
            InitializeComponent();
            _groupId = groupId;
            _user = new User(){Name = "Thomas"};
            _user.Groups.Add( new Group(){GroupId = 1, Name = "Familien SplitList"});
            Master = new MenuView();
            Detail = new NavigationPage(new MultiShopListView(groupId));
            MenuView.NavListView.ItemSelected += OnItemSelected;
            MenuView.NavListView1.ItemSelected += OnItemSelected;
        }

        private int _groupId;

        private User _user;
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
                    Detail = new NavigationPage(new MultiShopListView(_groupId));

                // Go to PantryView
                if(item.TargetType == typeof(Views.PantryView))
                    Detail = new NavigationPage( new PantryView());
                
                //Go to RecipeView
                if(item.TargetType == typeof(Views.RecipeView))
                    Detail = new NavigationPage(new RecipeView());

                // Go to UserView
                if (item.TargetType == typeof(Views.UserView))
                    Detail = new NavigationPage(new UserView(_user));

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