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
        public static ObservableCollection<ShoppingList> ListToObservableCollection(List<ShoppingListDTO> list)
        {
            ObservableCollection<ShoppingList> newShoppingListOC = new ObservableCollection<ShoppingList>();
            foreach (var shoppingListDto in list)
            {
                newShoppingListOC.Add(ShoppingListMapper.ShoppingListDtoToShoppingList(shoppingListDto));
            }

            return newShoppingListOC;
        }

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
