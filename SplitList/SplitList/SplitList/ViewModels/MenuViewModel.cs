using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using SplitList.Utility;
using SplitList.Views;
using Xamarin.Forms;

namespace SplitList.ViewModels
{
    public class MenuViewModel
    {
        public MenuViewModel()
        {
            
        }

        private ICommand _navigationCommand;
        public ICommand NavigationCommand
        {
            get => _navigationCommand ?? (_navigationCommand = new DelegateCommand(() =>
            {
                var newShoppingListView = new ShoppingListView();
                var newPantryView = new PantryView();

                switch(CommandParameter)
                {

                }
            });
        }
    }
}
