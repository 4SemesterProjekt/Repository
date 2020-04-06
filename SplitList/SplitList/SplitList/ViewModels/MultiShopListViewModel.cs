using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ApiFormat;
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
        public MultiShopListViewModel(INavigation navigation, Page page, int groupId)
        {
            Navigation = navigation;
            _page = page;
            GroupId = groupId;
            Lists = new ObservableCollection<ShoppingList>();
            Lists = ListMapper.ListToObservableCollection(SerializerShoppingList.GetShoppingListByGroupId(GroupId).Result);
        }

        #region Properties

        private Page _page;

        private bool deleteState;
        private INavigation Navigation { get; set; }

        private int _groupId;
        public int GroupId
        {
            get => _groupId;
            set => SetProperty(ref _groupId, value);
        }

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
            get => _listTappedCommand ?? (_listTappedCommand = new DelegateCommand(OpenShoppinglistExecute));
        }

        //Inserts UI-layer on top of the previous one, to easily implement navigation
        async void OpenShoppinglistExecute() 
        {
            await Navigation.PushAsync(new ShoppingListView(CurrentList));
        }

        private ICommand _addShoppingListCommand;

        public ICommand AddShoppingListCommand
        {
            get => _addShoppingListCommand ?? (_addShoppingListCommand = new DelegateCommand(AddShoppingListExecute));
        }

        private ICommand _deleteShoppingListCommand;

        public ICommand DeleteShoppingListCommand
        {
            get { return _deleteShoppingListCommand ?? (_deleteShoppingListCommand = new DelegateCommand(DeleteShoppingListExecute)); }
        }

        public async void DeleteShoppingListExecute()
        {
            if (!deleteState)
            {
                foreach (var shoppingList in Lists)
                {
                    shoppingList.IsVisible = true;
                }

                deleteState = true;
            }
            else if (deleteState)
            {
                bool IsAnyChecked = false;
                foreach (var shoppingList in Lists)
                {
                    if (shoppingList.IsChecked)
                    {
                        IsAnyChecked = true;
                        break;
                    }
                }

                if (IsAnyChecked)
                {
                    var result = await _page.DisplayAlert("Advarsel", "Er du sikker på slette disse lister?", "Ja", "Nej");
                    if (result)
                    {
                        for (int i = Lists.Count-1; i >= 0; i--)
                        {
                            if (Lists[i].IsChecked)
                            {
                                await SerializerShoppingList.DeleteShoppingList(ShoppingListMapper.ShoppingListToShoppingListDto(Lists[i]));
                                Lists.Remove(Lists[i]);
                                
                            }
                        }
                    }
                }
                foreach (var shoppingList in Lists)
                {
                    shoppingList.IsChecked = false;
                    shoppingList.IsVisible = false;
                }
                deleteState = false;
            }
        }

        //The function called when you click the add button to add a new shoppinglist
        //Adds the list if the ok button is pressed and the name is not null or empty
        //Does nothing if cancel is pressed
        async void AddShoppingListExecute()
        {
            string result = await _page.DisplayPromptAsync("New shoppingList","Enter a name for your shoppinglist");
            if(!string.IsNullOrEmpty(result))
            {
                var newList = new ShoppingList(result,GroupId);
                var listDTO = ShoppingListMapper.ShoppingListToShoppingListDto(newList);
                var listReturned = await SerializerShoppingList.PostShoppingList(listDTO);
                Lists.Add(ShoppingListMapper.ShoppingListDtoToShoppingList(listReturned));
            }
        }

        #endregion
    }
}
