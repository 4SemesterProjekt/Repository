using System.Collections.Generic;
using System.Linq;
using ApiFormat;
using ApiFormat.Item;
using ApiFormat.ShadowTables;
using ApiFormat.ShoppingList;
using ApiFormat.User;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Utilities;

namespace SplitListWebApi.Repositories.Implementation
{
    public class ShoppingListItemRepository
    {
        private SplitListContext _context;

        public ShoppingListItemRepository(SplitListContext context) => _context = context;

        public void CreateShoppingListItems(ItemModel itemModel, List<ShoppingListModel> shoppingListModels)
        {
            foreach (var shoppingListModel in shoppingListModels)
            {
                _context.ShoppingListItems.Add(new ShoppingListItem()
                {
                    ItemModel = itemModel,
                    ShoppingListModel = shoppingListModel,
                    ItemModelID = itemModel.ModelId,
                    ShoppingListModelID = shoppingListModel.ModelId
                });
            }
            _context.SaveChanges();
        }

        public void CreateShoppingListItems(List<ItemModel> itemModels, ShoppingListModel shoppingListModel)
        {
            if (itemModels == null) return;
            foreach (var itemModel in itemModels.Select(it => _context.Items.FirstOrDefault(im => im.ModelId == it.ModelId)))
            {
                _context.ShoppingListItems.Add(new ShoppingListItem()
                {
                    ItemModel = itemModel,
                    ShoppingListModel = shoppingListModel,
                    ItemModelID = itemModel.ModelId,
                    ShoppingListModelID = shoppingListModel.ModelId
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

        public List<ShoppingListItem> GetBy(int itemId, IEnumerable<int> shoppingListIds)
        {
            return null;
            /*return GeneralUtilities.GetFromDatabase(
                _context,
                selector: source => source,
                predi
                )*/
        }

        public List<ShoppingListItem> GetBy(IEnumerable<int> itemIds, int shoppingListId)
        {
            var shoppingListItems = new List<ShoppingListItem>();
            foreach (var itemId in itemIds)
            {
                shoppingListItems.Add(_context.ShoppingListItems.FirstOrDefault(sli => sli.ItemModelID == itemId && sli.ShoppingListModelID == shoppingListId));
            }

            return shoppingListItems;
        }
    }
}