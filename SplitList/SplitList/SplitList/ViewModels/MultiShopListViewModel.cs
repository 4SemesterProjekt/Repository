using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ApiFormat;
using ApiFormat.ShoppingList;
using ClientLibAPI;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation.Xaml;
using SplitList.Models;
using SplitList.Views;
using Xamarin.Forms;
namespace SplitList.ViewModels
{
    public class MultiShopListViewModel : BaseViewModel
    {
        public MultiShopListViewModel(INavigation nav, Page page, int groupId, string userId) : base(nav, page, groupId, userId)
        {
            Lists = new ObservableCollection<ShoppingList>();
        }

        #region Properties


        private bool deleteState;

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
            await Navigation.PushAsync(new ShoppingListView(CurrentList.ShoppingListId, GroupId, UserId));
        }

        private ICommand _addShoppingListCommand;

        public ICommand AddShoppingListCommand
        {
            get => _addShoppingListCommand ?? (_addShoppingListCommand = new DelegateCommand(AddShoppingListExecute));
        }

        //The function called when you click the add button to add a new shoppinglist
        //Adds the list if the ok button is pressed and the name is not null or empty
        //Does nothing if cancel is pressed
        async void AddShoppingListExecute()
        {
            string result = await Page.DisplayPromptAsync("New shoppingList", "Enter a name for your shoppinglist");
            if (!string.IsNullOrEmpty(result))
            {
                var newList = new ShoppingList(result, GroupId);
                var listDTO = mapper.Map<ShoppingListDTO>(newList);
                var listReturned = await SerializerShoppingList.CreateShoppingList(listDTO);
                Lists.Add(mapper.Map<ShoppingList>(listReturned));
            }
        }

        private ICommand _deleteShoppingListCommand;

        public ICommand DeleteShoppingListCommand
        {
            get { return _deleteShoppingListCommand ?? (_deleteShoppingListCommand = new DelegateCommand(DeleteShoppingListExecute)); }
        }

        /// <summary>
        /// On first press shows a checkbox next to each shoppinglist
        /// On second press, if any checkbox is checked prompts the user to confirm deletions, if chosen any selected shoppinglist will be deleted
        /// </summary>
        public async void DeleteShoppingListExecute()
        {
            if (!deleteState) //Deletestate is used to check if the checkboxes should be shown
            {
                foreach (var shoppingList in Lists)
                {
                    shoppingList.IsVisible = true;
                }

                deleteState = true;
            }
            else if (deleteState)
            {
                bool IsAnyChecked = false; //Does a check through the lists to see if any box is checked, makes sure that there is no promt when no deletion is attemted
                foreach (var shoppingList in Lists)
                {
                    if (shoppingList.IsChecked)
                    {
                        IsAnyChecked = true;
                        break;
                    }
                }

                if (IsAnyChecked)
                {//Prompts the user to confirm the deletion on the selected shoppinglists
                    var result = await Page.DisplayAlert("Warning", "Are you sure that you want to delete the selected shoppinglists", "Yes", "No");
                    if (result)
                    {
                        for (int i = Lists.Count-1; i >= 0; i--)
                        {
                            if (Lists[i].IsChecked)
                            {//Serializer function deletes the selected shoppinglists on the database
                                await SerializerShoppingList.DeleteShoppingList(mapper.Map<ShoppingListDTO>(Lists[i]));
                                Lists.Remove(Lists[i]);
                                
                            }
                        }
                    }
                }
                //Hides the checkboxes after use
                foreach (var shoppingList in Lists)
                {
                    shoppingList.IsChecked = false;
                    shoppingList.IsVisible = false;
                }
                deleteState = false;
            }
        }

        public override async void OnAppearingExecute()
        {
            Group group = mapper.Map<Group>(await SerializerGroup.GetGroupById(GroupId));
            Lists = group.ShoppingLists;
        }

        #endregion
    }
}
