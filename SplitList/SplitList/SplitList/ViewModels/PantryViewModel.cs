using System;
using System.Windows.Input;
using ClientLibAPI;
using Prism.Commands;
using Prism.Mvvm;
using SplitList.Mapping;
using SplitList.Models;
using Xamarin.Forms;

namespace SplitList.ViewModels
{
    public class PantryViewModel : BindableBase
    {
        public PantryViewModel()
        {
            Pantry = new Pantry();
           // Pantry = PantryMapper.PantryDtoToPantry(SerializerPantry.GetPantryByGroupId(1).Result);
        }

        #region Properties

        private bool _deleteState = false;
        public Page Page { get; set; }

        private Pantry _pantry;
        public Pantry Pantry
        {
            get => _pantry;
            set => SetProperty(ref _pantry, value);
        }

        #endregion

        #region Commands

        private ICommand _addItemToListCommand;

        public ICommand AddItemToListCommand
        {
            get
            {
                return _addItemToListCommand ?? (_addItemToListCommand = new DelegateCommand<object>(AddItemToListCommandExecute));
            }
        }

        public void AddItemToListCommandExecute(object obj)
        {
            Pantry.Items.Add(new Item("",1,obj as CheckBox));
        }

        private ICommand _deleteItemCommand;

        public ICommand DeleteItemCommand
        {
            get { return _deleteItemCommand ?? (_deleteItemCommand = new DelegateCommand(DeleteItemExecute)); }
        }

        /// <summary>
        /// On first press shows a checkbox next to each item
        /// On second press, if any checkbox is checked prompts the user to confirm deletions, if chosen any selected Items will be deleted
        /// </summary>
        public async void DeleteItemExecute()
        {
            if (!_deleteState)
            {
                foreach (var pantryItem in Pantry.Items)
                {
                    pantryItem.IsVisible = true;
                }

                _deleteState = true;
            }

            else if (_deleteState)
            {
                bool isAnyChecked = false;

                foreach (var pantryItem in Pantry.Items)
                {
                    if (pantryItem.IsChecked)
                    {
                        isAnyChecked = true;
                        break;
                    }
                }

                if (isAnyChecked)
                {
                    var result = await Page.DisplayAlert("Warning",
                        "Are you sure that you want to delete the selected items?", "Yes", "No");
                    if (result)
                    {
                        for (int i = Pantry.Items.Count - 1; i >= 0; i--)
                        {
                            if(Pantry.Items[i].IsChecked)
                                Pantry.Items.RemoveAt(i);
                        }
                    }
                }

                foreach (var pantryItem in Pantry.Items)
                {
                    pantryItem.IsChecked = false;
                    pantryItem.IsVisible = false;
                }
            }
        }
        #endregion
    }
}