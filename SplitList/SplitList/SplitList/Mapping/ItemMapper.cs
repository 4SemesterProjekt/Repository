using System;
using System.Collections.Generic;
using System.Text;
using ApiFormat;
using SplitList.Models;

namespace SplitList.Mapping
{
    public class ItemMapper
    {
        /// <summary>
        /// Maps a GUI item to an item transferobject
        /// </summary>
        /// <param name="item"> Type item</param>
        /// <returns>ItemDTO</returns>
        public static ItemDTO ItemToItemDto(Item item)
        {
            ItemDTO newItemDto = new ItemDTO
            {
                ItemID = item.ItemId, 
                Name = item.Name, 
                Amount = item.Amount, 
                Type = item.Category
            };
            return newItemDto;
        }
        /// <summary>
        /// Maps a transfer item object to a GUI item
        /// </summary>
        /// <param name="itemDto">Type ItemDTO</param>
        /// <returns>Item</returns>
        public static Item ItemDtoToItem(ItemDTO itemDto)
        {
            Item newItem = new Item
            {
                ItemId = itemDto.ItemID, 
                Name = itemDto.Name, 
                Amount = itemDto.Amount, 
                Category = itemDto.Type
            };
            return newItem;
        }
    }
}