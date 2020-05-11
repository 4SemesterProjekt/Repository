using System;
using System.Collections.Generic;
using System.Linq;
using ApiFormat;
using ApiFormat.Item;
using ApiFormat.ShadowTables;
using ApiFormat.ShoppingList;
using ApiFormat.User;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Services.Interfaces;
using SplitListWebApi.Utilities;

namespace SplitListWebApi.Repositories.Implementation
{
    public class ShoppingListItemRepository
    {
        private SplitListContext _context;
        public ShoppingListItemRepository(SplitListContext context) => _context = context;

        public void CreateShoppingListItems(List<ItemDTO> itemDTOs, ShoppingListModel shoppingListModel)
        {
            if (itemDTOs == null) throw new ArgumentNullException("ItemDTOs passed was null.");

            foreach (ItemModel itemModel in itemDTOs.Select(dto => _context.Items.FirstOrDefault(im => im.ModelId == dto.ModelId)))
            {
                int amount = itemDTOs.Where(dto => dto.ModelId == itemModel.ModelId).Select(dto => dto.Amount).FirstOrDefault();

                _context.ShoppingListItems.Add(new ShoppingListItem()
                {
                    ItemModel = itemModel,
                    ShoppingListModel = shoppingListModel,
                    ItemModelID = itemModel.ModelId,
                    ShoppingListModelID = shoppingListModel.ModelId,
                    Amount = amount
                });
            }
            _context.SaveChanges();
        }

        public void DeleteShoppingListItems(List<ShoppingListItem> shoppingListItems)
        {
            if (shoppingListItems != null)
            {
                _context.ShoppingListItems.RemoveRange(shoppingListItems);
                _context.SaveChanges();
            }
        }
    }
}