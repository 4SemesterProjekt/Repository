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
            Master = new MenuView();
            Detail = new NavigationPage(new MultiShopListView(groupId));
            MenuView.NavListView.ItemSelected += OnItemSelected;
        }
        /// <summary>
        /// Navigates the page to the selected page from the menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MenuViewItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                MenuView.NavListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}