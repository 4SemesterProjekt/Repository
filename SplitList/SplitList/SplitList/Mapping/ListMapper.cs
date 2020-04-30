using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ApiFormat;
using SplitList.Models;

namespace SplitList.Mapping
{
    public class ListMapper
    {
        //Maps a list of shoppinglist transferobjects to an observable collection of shoppinglists
        //Used to recieve a groups shoppinglists translated for use
        public static ObservableCollection<ShoppingList> ListToObservableCollection(List<ShoppingListDTO> list)
        {
            ObservableCollection<ShoppingList> newShoppingListOC = new ObservableCollection<ShoppingList>();
            foreach (var shoppingListDto in list)
            {
                newShoppingListOC.Add(ShoppingListMapper.ShoppingListDtoToShoppingList(shoppingListDto));
            }

            return newShoppingListOC;
        }

        //Maps observablecollection of shopping lists back to list of transforobjects to return to the database
        public static List<ShoppingListDTO> ObservableCollectionToList(ObservableCollection<ShoppingList> collection)
        {
            List<ShoppingListDTO> newList = new List<ShoppingListDTO>();
            foreach (var shoppingList in collection)
            {
                newList.Add(ShoppingListMapper.ShoppingListToShoppingListDto(shoppingList));
            }

            return newList;
        }
    }
}
