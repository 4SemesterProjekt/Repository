using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Prism.Mvvm;
using SplitList.Utility;

namespace SplitList.ViewModels
{
    public class MenuViewModel : BindableBase
    {
        public MenuViewModel()
        {
            _navigationItems = new ObservableCollection<NavigationItem>();
            _navigationItems.Add(new NavigationItem());
        }

        public ObservableCollection<NavigationItem> _navigationItems;

        public ObservableCollection<NavigationItem> NavigationItems
        {
            get => _navigationItems; 
            set => SetProperty(ref _navigationItems, value);
        }
    }
}
