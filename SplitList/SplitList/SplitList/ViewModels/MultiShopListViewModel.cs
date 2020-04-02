﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ClientLibAPI;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation.Xaml;
using SplitList.Mapping;
using SplitList.Models;
using SplitList.Views;
using Xamarin.Forms;
namespace SplitList.ViewModels
{
    public class MultiShopListViewModel : BindableBase
    {
        public MultiShopListViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Lists = new ObservableCollection<ShoppingList>();
            Lists = ListMapper.ListToObservableCollection(SerializerShoppingList.GetShoppingListByGroupId(1));
        }

        #region Properties
        private INavigation Navigation { get; set; }

        private ObservableCollection<ShoppingList> _lists;

        public ObservableCollection<ShoppingList> Lists
        {
            get => _lists;
            set => SetProperty(ref _lists, value);
        }

        private ShoppingList _currentList;

        public ShoppingList CurrentList
        {
            get => _currentList;
            set => SetProperty(ref _currentList, value);
        }

        
        #endregion

        #region Commands

        private ICommand _listTappedCommand;

        public ICommand ListTappedCommand
        {
            get => _listTappedCommand ?? (_listTappedCommand = new DelegateCommand(NavigationDownExecute));
        }

        async void NavigationDownExecute() //Inserts UI-layer on top of the previous one, to easily implement navigation.
        {
            await Navigation.PushAsync(new ShoppingListView(CurrentList));
        }

        #endregion
    }
}
