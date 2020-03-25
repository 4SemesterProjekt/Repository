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
    public class MenuViewModel : BindableBase
    {
        public MenuViewModel()
        {
        }

        #region Properties


        #endregion

        #region Commands

        private ICommand _navgiationCommand;

        public ICommand NavigationCommand
        {
            get { return _navgiationCommand ?? (_navgiationCommand = new DelegateCommand(() =>
            {
                var navPage = new MasterDetailPage()
                {
                    Master = new MenuView() {Title = "MenuView"},
                    Detail = new NavigationPage(new PantryView())
                };
                Application.Current.MainPage = navPage;
                
            })); }
        }

        #endregion
        
    }
}
