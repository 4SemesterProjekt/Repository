using System;
using System.Collections.Generic;
using System.Text;
using ApiFormat;
using SplitList.Models;

namespace SplitList.Mapping
{
    class ShoppingListMapper
    {
        //Takes a shoppinglist transfer object in and maps it to the shoppinglists used by the program
        //Parameter ShoppinglistDTO object
        //Returns a shoppinglist object
        public static ShoppingList ShoppingListDtoToShoppingList(ShoppingListDTO dto)
        {
            ShoppingList newList = new ShoppingList();
            newList.Name = dto.shoppingListName;
            newList.ShoppingListId = dto.shoppingListID;
            newList.GroupId = dto.shoppingListGroupID;
            if (dto.Items != null)
            {
                foreach (var dtoItem in dto.Items)
                {
                    newList.Items.Add(ItemMapper.ItemDtoToItem(dtoItem));
                }
            }
            return newList;
        }

        //Takes a shoppinglist object and maps it to a transfer object for publishing to the database
        //Parameter ShoppingList
        //Returns ShoppinglistDTO
        public static ShoppingListDTO ShoppingListToShoppingListDto(ShoppingList list)
        {
            ShoppingListDTO newDtoList = new ShoppingListDTO();
            newDtoList.Items = new List<ItemDTO>();
            newDtoList.shoppingListName = list.Name;
            newDtoList.shoppingListID = list.ShoppingListId;
            newDtoList.shoppingListGroupID = list.GroupId;
            if (list.Items != null)
            {
                foreach (var listItem in list.Items)
                {
                    newDtoList.Items.Add(ItemMapper.ItemToItemDto(listItem));
                }
            }
            return newDtoList;
        }
    }
}
