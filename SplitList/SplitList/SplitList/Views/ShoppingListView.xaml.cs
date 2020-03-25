using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplitList.Models;
using SplitList.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SplitList.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShoppingListView : ContentPage
    {
        public ShoppingListView()
        {
            InitializeComponent();
        }

        //private void BtnAddItem_OnReleased(object sender, EventArgs e)
        //{
        //    var vm = BindingContext as ShoppingListViewModel;
        //    vm.Items.Add(new Item("", 1));
        //}
    }
}