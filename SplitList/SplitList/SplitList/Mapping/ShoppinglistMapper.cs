using System;
using System.Collections.Generic;
using System.Text;
using ApiFormat;
using SplitList.Models;

namespace SplitList.Mapping
{
    class ShoppingListMapper
    {
        public static ShoppingList ShoppingListDtoToShoppingList(ShoppingListDTO dto)
        {
            ShoppingList newList = new ShoppingList();
            newList.Name = dto.shoppingListName;
            newList.ShoppingListId = dto.shoppingListID;
            if (dto.Items != null)
            {
                foreach (var dtoItem in dto.Items)
                {
                    newList.Items.Add(ItemMapper.ItemDtoToItem(dtoItem));
                }
            }
            return newList;
        }

        public static ShoppingListDTO ShoppingListToShoppingListDto(ShoppingList list)
        {
            ShoppingListDTO newDtoList = new ShoppingListDTO();
            newDtoList.Items = new List<ItemDTO>();
            newDtoList.shoppingListName = list.Name;
            newDtoList.shoppingListID = list.ShoppingListId;
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
