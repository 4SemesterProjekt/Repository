using System;
using System.Collections.Generic;
using System.Linq;
using ApiFormat;
using ApiFormat.Item;
using ApiFormat.Pantry;
using ApiFormat.ShadowTables;
using ApiFormat.ShoppingList;
using ApiFormat.User;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Services.Interfaces;
using SplitListWebApi.Utilities;

namespace SplitListWebApi.Repositories.Implementation
{
    public class PantryItemRepository
    {
        private SplitListContext _context;
        public PantryItemRepository(SplitListContext context) => _context = context;

        public void CreatePantryItems(List<ItemDTO> itemDTOs, PantryModel pantryModel)
        {
            if (itemDTOs == null) throw new ArgumentNullException("ItemDTOs passed was null.");

            foreach (ItemModel itemModel in itemDTOs.Select(dto => _context.Items.FirstOrDefault(im => im.ModelId == dto.ModelId)))
            {
                int amount = itemDTOs.Where(dto => dto.ModelId == itemModel.ModelId).Select(dto => dto.Amount).FirstOrDefault();

                _context.PantryItems.Add(new PantryItem()
                {
                    ItemModel = itemModel,
                    PantryModel = pantryModel,
                    ItemModelID = itemModel.ModelId,
                    PantryModelID = pantryModel.ModelId,
                    Amount = amount
                });
            }
            _context.SaveChanges();
        }

        public void DeletePantryItems(List<PantryItem> pantryItems)
        {
            if (pantryItems != null)
            {
                _context.PantryItems.RemoveRange(pantryItems);
                _context.SaveChanges();
            }
        }
    }
}