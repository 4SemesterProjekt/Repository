using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiFormat;
using ClientLibAPI;
using SplitList.Mapping;
using SplitList.Models;
using SplitList.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SplitList.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShoppingListView : ContentPage
    {
        public ShoppingListView(ShoppingList shoppingList)
        {
            InitializeComponent();
            ShoppingListViewModel.Page = this;
            ShoppingListViewModel.ShoppingList = ShoppingListMapper.ShoppingListDtoToShoppingList(SerializerShoppingList.GetShoppingListByShoppinglistId(shoppingList.ShoppingListId).Result);
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            ShoppingListDTO tobj = ShoppingListMapper.ShoppingListToShoppingListDto(ShoppingListViewModel.ShoppingList);
            var result = await SerializerShoppingList.PostShoppingList(tobj);
        }

    }
}