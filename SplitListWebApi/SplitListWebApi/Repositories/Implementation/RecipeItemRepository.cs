using System;
using System.Collections.Generic;
using System.Linq;
using ApiFormat.Item;
using ApiFormat.Recipe;
using ApiFormat.ShadowTables;
using ApiFormat.ShoppingList;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Services.Interfaces;

namespace SplitListWebApi.Repositories.Implementation
{
    public class RecipeItemRepository
    {
        private readonly SplitListContext _context;
        public RecipeItemRepository(SplitListContext context) => _context = context;

        public void CreateRecipeItems(List<ItemDTO> itemDTOs, RecipeModel recipeModel)
        {
            if (itemDTOs == null) throw new ArgumentNullException("ItemDTOs passed was null.");

            foreach (ItemModel itemModel in itemDTOs.Select(dto => _context.Items.FirstOrDefault(im => im.ModelId == dto.ModelId)))
            {
                int amount = itemDTOs.Where(dto => dto.ModelId == itemModel.ModelId).Select(dto => dto.Amount).FirstOrDefault();

                _context.RecipeItems.Add(new RecipeItem()
                {
                    ItemModel = itemModel,
                    RecipeModel = recipeModel,
                    ItemModelID = itemModel.ModelId,
                    RecipeModelID = recipeModel.ModelId,
                    Amount = amount
                });
            }
            _context.SaveChanges();
        }

        public void DeleteRecipeItems(List<RecipeItem> recipeItems)
        {
            if (recipeItems == null) return;

            _context.RecipeItems.RemoveRange(recipeItems);
            _context.SaveChanges();
        }
    }
}